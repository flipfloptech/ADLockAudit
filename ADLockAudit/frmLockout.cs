using Cassia;
using Microsoft.Management.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Eventing.Reader;
using System.DirectoryServices.AccountManagement;
using System.DirectoryServices.ActiveDirectory;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ADLockAudit
{
    public partial class frmLockout : Form
    {
        private CancellationTokenSource _tokenSource = null;
        private CancellationToken _token;
        private Task _analysisTask = null;
        private DateTime _taskStart;
        private const string _szUsernamePlaceholder = @"DOMAIN\Username, username@domain.local, username";
        private Domain machineDomain = null;
        private readonly TreeNode _DomainControllers = null;
        private readonly TreeNode _MemberServers = null;
        private readonly Dictionary<int, string> _StatusLookup = new Dictionary<int, string>() {
            { unchecked ( 0x00000002 ), "A user logged on to this computer" },
            { unchecked ( 0x00000003 ), "A user or computer logged on to this computer from the network (HASHED) (NETHASHED)" },
            { unchecked ( 0x00000004 ), "Batch logon type is used by batch servers, where processes may be executing on behalf of a user without their direct intervention" },
            { unchecked ( 0x00000005 ), "A service was started by the Service Control Manager" },
            { unchecked ( 0x00000007 ), "This workstation was unlocked" },
            { unchecked ( 0x00000008 ), "A user or computer logged on to this computer from the network (UNHASHED) (NETHASHED)" },
            { unchecked ( 0x00000009 ), "A caller cloned its current token and specified new credentials for outbound connections (SAMELOCAL) (DIFFNET)" },
            { unchecked ( 0x00000010 ), "A user logged on to this computer remotely using Terminal Services or Remote Desktop" },
            { unchecked ( 0x00000011 ), "A user logged on to this computer with network credentials that were stored locally on the computer. (NODC)" },
            { unchecked ( (int)0xC000005E ), "There are currently no logon servers available to service the logon request" },
            { unchecked ( (int)0xC0000064 ), "User logon with misspelled or bad user account" },
            { unchecked ( (int)0xC000006A ), "User logon with misspelled or bad password" },
            { unchecked ( (int)0xC000006D ), "This is either due to a bad username or authentication information" },
            { unchecked ( (int)0xC000006E ), "Unknown user name or bad password" },
            { unchecked ( (int)0xC000006F ), "User logon outside authorized hours" },
            { unchecked ( (int)0xC0000070 ), "User logon from unauthorized workstation" },
            { unchecked ( (int)0xC0000071 ), "User logon with expired password" },
            { unchecked ( (int)0xC0000072 ), "User logon to account disabled by administrator" },
            { unchecked ( (int)0xC00000DC ), "Indicates the Sam Server was in the wrong state to perform the desired operation" },
            { unchecked ( (int)0xC0000133 ), "Clocks between DC and other computer too far out of sync" },
            { unchecked ( (int)0xC000015B ), "The user has not been granted the requested logon type (aka logon right) at this machine" },
            { unchecked ( (int)0xC000018C ), "The logon request failed because the trust relationship between the primary domain and the trusted domain failed" },
            { unchecked ( (int)0xC0000192 ), "An attempt was made to logon ), but the Netlogon service was not started" },
            { unchecked ( (int)0xC0000193 ), "User logon with expired account" },
            { unchecked ( (int)0xC0000224 ), "User is required to change password at next logon" },
            { unchecked ( (int)0xC0000225 ), "Evidently a bug in Windows and not a risk" },
            { unchecked ( (int)0xC0000234 ), "User logon with account locked" },
            { unchecked ( (int)0xC00002EE ), "Failure Reason: An Error occurred during Logon" },
            { unchecked ( (int)0xC0000413 ), "Logon Failure: The machine you are logging onto is protected by an authentication firewall. The specified account is not allowed to authenticate to the machine" },
            { unchecked ( 0x0 ), "Status OK" }
        };
        private readonly Dictionary<int, string> _EventTypeLookup = new Dictionary<int, string>() {
            { 4624, "Logon Successful" },
            { 4625, "Logon Failed" }, // use status lookup for reason
            { 4740, "Locked Out User Account" },
            { 6279, "Locked Out User Account (NPS)" },
            { 528, "Logon Successful (OK) (2K3)" },
            { 529, "Logon Failed (Unknown Username / Bad Password) (2K3) " },
            { 530, "Logon Failed (Account logon time restriction violation) (2K3)" },
            { 531, "Logon Failed (Account currently disabled) (2K3)" },
            { 532, "Logon Failed (The specified user account has expired) (2K3)" },
            { 533, "Logon Failed (User not allowed to logon at this computer) (2K3)" },
            { 534, "Logon Failed (The user has not been granted the requested logon type at this machine) (2K3)" },
            { 535, "Logon Failed (The specified account's password has expired) (2K3)" },
            { 536, "Logon Failed (The NetLogon component is not active) (2K3)" },
            { 537, "Logon Failed (The logon attempt failed for other reasons) (2K3)" },
            { 538, "Logoff (OK) (2K3)" },
            { 539, "Logon Failed (Locked) (2K3)" },
            { 644, "Locked Out User Account (Locked) (2K3)" },
        };
        public frmLockout()
        {
            InitializeComponent();
            dateTimeStart.Format = DateTimePickerFormat.Custom;
            dateTimeStart.CustomFormat = "MM/dd/yyyy hh:mm tt";
            dateTimeEnd.Format = DateTimePickerFormat.Custom;
            dateTimeEnd.CustomFormat = "MM/dd/yyyy hh:mm tt";
            dateTimeEnd.Value = DateTime.Now;
            dateTimeStart.Value = dateTimeEnd.Value.Subtract(TimeSpan.FromDays(7));
            _DomainControllers = viewMachines.Nodes.Add("Controllers");
            _MemberServers = viewMachines.Nodes.Add("Members");
            viewMachines.TopNode = _DomainControllers;
            SafeUpdateUI(false);
            Status("v0.0001a Initialized");
        }

        private void frmLockout_Load(object sender, EventArgs e)
        {

        }
        private bool ControllerEnumeration(Domain enumDomain)
        {
            Status("Enumerating Domain Controllers");
            try
            {
                foreach (DomainController _DC in enumDomain.DomainControllers)
                {
                    _DomainControllers.Nodes.Add(_DC.Name).Tag = _DC;
                }
            }
            catch (Exception _ex)
            {
                Status($"Error enumerating controllers - PROVIDE TO SUPPORT - UNKNKOWN ({_ex.GetType().Name}:{_ex.HResult})");
                return false;
            }
            Status("Enumerated Domain Controllers");
            return true;
        }

        private bool checkNode(TreeNode _node, string szSearch)
        {
            for (int i = 0; i < _node.Nodes.Count; i++)
            {
                if (_node.Nodes[i].Text == szSearch)
                {
                    return true;
                }
            }
            return false;
        }
        private bool MemberEnumeration(Domain enumDomain)
        {
            Status("Enumerating member servers");
            try
            {
                PrincipalContext _pContext = new PrincipalContext(ContextType.Domain, enumDomain.Name);
                ComputerPrincipal _cPrincipal = new ComputerPrincipal(_pContext);
                PrincipalSearcher _pSearcher = new PrincipalSearcher();

                _cPrincipal.Name = "*";
                _pSearcher.QueryFilter = _cPrincipal;

                PrincipalSearchResult<Principal> _pResults = _pSearcher.FindAll();
                foreach (ComputerPrincipal _member in _pResults)
                {
                    if (!checkNode(_DomainControllers, $"{_member.Name}.{enumDomain.Name}"))
                    {
                        _MemberServers.Nodes.Add($"{_member.Name}.{enumDomain.Name}").Tag = _member;
                    }
                }
            }
            catch (Exception _ex)
            {
                Status($"Error enumerating member services - PROVIDE TO SUPPORT - UNKNKOWN ({_ex.GetType().Name}:{_ex.HResult})");
                return false;
            }
            Status("Enumerated member servers");
            return true;
        }
        private delegate void SafeCallDelegateString(string Value);
        private void Status(string szStatus)
        {
            if (lblStatus.GetCurrentParent().InvokeRequired)
            {
                SafeCallDelegateString d = new SafeCallDelegateString(Status);
                Invoke(d, new object[] { szStatus });
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(szStatus))
                {
                    lblStatus.Text = $"Status: {szStatus}";
                }

                Update();
            }
        }

        private void frmLockout_Shown(object sender, EventArgs e)
        {
            Update();
            try
            {
                Status("Gathering current domain info");
                machineDomain = Domain.GetComputerDomain();
                lblCurrentDomain.Text = machineDomain.Name;
            }
            catch (ActiveDirectoryObjectNotFoundException)
            {
                lblCurrentDomain.Text = "<<< ERROR >>>";
                Status($"Error gathering domain info - Object Not Found");
                return;
            }
            catch (UnauthorizedAccessException)
            {
                lblCurrentDomain.Text = "<<< ERROR >>>";
                Status($"Error gathering domain info - Unauthorized");
                return;
            }
            catch (Exception _ex)
            {
                lblCurrentDomain.Text = "<<< ERROR >>>";
                Status($"Error gathering domain info - PROVIDE TO SUPPORT - UNKNKOWN ({_ex.GetType().Name}:{_ex.HResult})");
                return;
            }
            if (!ControllerEnumeration(machineDomain))
            {
                return;
            }

            if (!MemberEnumeration(machineDomain))
            {
                return;
            }

            viewMachines.Sort();
            SafeUpdateUI(true);
        }

        private void viewMachines_AfterCheck(object sender, TreeViewEventArgs e)
        {
            viewMachines.BeginUpdate();
            if (e.Node.Parent == null && ((e.Action == TreeViewAction.ByMouse) || (e.Action == TreeViewAction.ByKeyboard)))
            {
                foreach (TreeNode _node in e.Node.Nodes)
                {
                    _node.Checked = e.Node.Checked;
                }
            }
            else if (e.Node.Parent != null && ((e.Action == TreeViewAction.ByMouse) || (e.Action == TreeViewAction.ByKeyboard)))
            {
                bool _checkParent = false;
                foreach (TreeNode _node in e.Node.Parent.Nodes)
                {
                    if (!_node.Checked)
                    {
                        _checkParent = false;
                        break;
                    }
                    else if (_node.Checked)
                    {
                        _checkParent = true;
                    }
                }
                if (e.Node.Parent.Checked != _checkParent)
                {
                    e.Node.Parent.Checked = _checkParent;
                }
            }
            viewMachines.EndUpdate();
            UpdateUI();
        }

        private void txtUsername_Enter(object sender, EventArgs e)
        {
            if (txtUsername.Text == _szUsernamePlaceholder)
            {
                txtUsername.Text = string.Empty;
                txtUsername.ForeColor = SystemColors.WindowText;
            }
            UpdateUI();
        }

        private void txtUsername_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUsername.Text))
            {
                txtUsername.Text = _szUsernamePlaceholder;
                txtUsername.ForeColor = SystemColors.GrayText;
            }
            UpdateUI();
        }

        private void txtUsername_TextChanged(object sender, EventArgs e)
        {
            UpdateUI();
        }

        private bool isTreeViewChecked(TreeNodeCollection _Nodes)
        {
            foreach (TreeNode _check in _Nodes)
            {
                if (_check.Checked)
                {
                    return true;
                }

                if (_check.Nodes.Count > 0)
                {
                    if (isTreeViewChecked(_check.Nodes))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        private void UpdateUI()
        {
            if (!isValidDateRange(dateTimeStart.Value, dateTimeEnd.Value) || !isTreeViewChecked(viewMachines.Nodes) || (string.IsNullOrWhiteSpace(txtUsername.Text) || (txtUsername.Text == _szUsernamePlaceholder)))
            {
                btnRunAnalysis.Enabled = false;
            }
            else
            {
                btnRunAnalysis.Enabled = true;
            }
            Update();
        }

        private bool isValidDateRange(DateTime _start, DateTime _stop)
        {
            if (_start > _stop)
            {
                return false;
            }

            return true;
        }

        private void btnRunAnalysis_Click(object sender, EventArgs e)
        {
            if (btnRunAnalysis.Text == "Run Analysis")
            {
                UserPrincipal _lockedOut = FindUserPrincipal(machineDomain, txtUsername.Text);
                if (_lockedOut == null)
                {
                    Status("Unable to locate username / invalid username");
                    return;
                }
                progressBar.Value = 0;
                progressBar.Maximum = 0;
                lstEvents.Items.Clear();
                lstServices.Items.Clear();
                lstSignedOn.Items.Clear();
                lblEnabled.Text = _lockedOut.Enabled.ToString();
                lblLocked.Text = _lockedOut.IsAccountLockedOut().ToString();
                lblEnabled.ForeColor = lblLockoutTime.ForeColor;
                lblLocked.ForeColor = lblEnabled.ForeColor;
                SafeUpdateUI(false);
                if (!string.IsNullOrWhiteSpace(_lockedOut.AccountLockoutTime.ToString()))
                {
                    lblLockoutTime.Text = _lockedOut.AccountLockoutTime.ToString();
                }
                else
                {
                    lblLockoutTime.Text = "NOT LOCKED";
                }

                lblBadLogonCount.Text = _lockedOut.BadLogonCount.ToString();

                if (!string.IsNullOrWhiteSpace(_lockedOut.AccountExpirationDate.ToString()))
                {
                    lblExpirationDate.Text = _lockedOut.AccountExpirationDate.ToString();
                }
                else
                {
                    lblExpirationDate.Text = "NEVER EXPIRES";
                }

                if (!string.IsNullOrWhiteSpace(_lockedOut.LastBadPasswordAttempt.ToString()))
                {
                    lblLastBadPassword.Text = _lockedOut.LastBadPasswordAttempt.ToString();
                }
                else
                {
                    lblLastBadPassword.Text = "NEVER WRONG";
                }

                if (!string.IsNullOrWhiteSpace(_lockedOut.LastLogon.ToString()))
                {
                    lblLastLogon.Text = _lockedOut.LastLogon.ToString();
                }
                else
                {
                    lblLastLogon.Text = "NEVER LOGGED ON";
                }

                if (!string.IsNullOrWhiteSpace(_lockedOut.LastPasswordSet.ToString()))
                {
                    lblLastPasswordSet.Text = _lockedOut.LastPasswordSet.ToString();
                }
                else
                {
                    lblLastPasswordSet.Text = "NEVER SET";
                }

                string[] _machines = SelectedMachines(viewMachines.Nodes);
                if (_machines.Count() > 0)
                {
                    _taskStart = DateTime.UtcNow;
                    _tokenSource = new CancellationTokenSource();
                    _token = _tokenSource.Token;
                    _analysisTask = Task.Factory.StartNew(() =>
                    {
                        RunAnalysis(_machines, _lockedOut, _token);
                    }, _token);
                    Status("Running analysis");
                    btnRunAnalysis.Text = "Cancel Analysis";
                }
                else
                {
                    Status("Unable to run analysis, no machines selected.");
                    return;
                }
            }
            else
            {
                if (_token != null)
                {
                    _tokenSource.Cancel();
                    btnRunAnalysis.Text = "Cancelling";
                    btnRunAnalysis.Enabled = false;
                }
            }
        }
        private delegate void SafeCallDelegateSort(ListView _view);
        private void SafeSort(ListView _view)
        {
            if (_view.InvokeRequired)
            {
                SafeCallDelegateSort d = new SafeCallDelegateSort(SafeSort);
                Invoke(d, new object[] { _view });
            }
            else
            {
                _view.Sort();
            }
        }
        private delegate void SafeCallDelegateUpdate(ListView _view);
        private void SafeBeginUpdate(ListView _view)
        {
            if (_view.InvokeRequired)
            {
                SafeCallDelegateUpdate d = new SafeCallDelegateUpdate(SafeBeginUpdate);
                Invoke(d, new object[] { _view });
            }
            else
            {
                _view.BeginUpdate();
            }
        }
        private void SafeEndUpdate(ListView _view)
        {
            if (_view.InvokeRequired)
            {
                SafeCallDelegateUpdate d = new SafeCallDelegateUpdate(SafeEndUpdate);
                Invoke(d, new object[] { _view });
            }
            else
            {
                _view.EndUpdate();
            }
        }
        private delegate void SafeCallDelegateInt(int Value);
        private void SafeSetProgressBarValue(int Value)
        {
            if (progressBar.GetCurrentParent().InvokeRequired)
            {
                SafeCallDelegateInt d = new SafeCallDelegateInt(SafeSetProgressBarValue);
                Invoke(d, new object[] { Value });
            }
            else
            {
                progressBar.Value = Value;
            }
        }
        private void SafeSetProgressBarMax(int Value)
        {
            if (progressBar.GetCurrentParent().InvokeRequired)
            {
                SafeCallDelegateInt d = new SafeCallDelegateInt(SafeSetProgressBarMax);
                Invoke(d, new object[] { Value });
            }
            else
            {
                progressBar.Maximum = Value;
            }
        }
        private delegate void SafeCallDelegateAddItem(ListView _view, ListViewItem _item);
        private void SafeAddItem(ListView _view, ListViewItem _item)
        {
            if (_view.InvokeRequired)
            {
                SafeCallDelegateAddItem d = new SafeCallDelegateAddItem(SafeAddItem);
                Invoke(d, new object[] { _view, _item });
            }
            else
            {
                _view.Items.Add(_item);
            }
        }
        private delegate void SafeCallDelegateSetButtonText(Button _button, string szCaption);
        private void SafeSetButtonText(Button _button, string szCaption)
        {
            if (_button.InvokeRequired)
            {
                SafeCallDelegateSetButtonText d = new SafeCallDelegateSetButtonText(SafeSetButtonText);
                Invoke(d, new object[] { _button, szCaption });
            }
            else
            {
                _button.Text = szCaption;
            }
        }
        private void RunAnalysis(string[] _machines, UserPrincipal _lockedOut, CancellationToken _cancel)
        {

            SafeSetProgressBarMax(_machines.Count());
            if (!_cancel.IsCancellationRequested)
            {
                foreach (string _machine in _machines)
                {
                    if (_cancel.IsCancellationRequested)
                    {
                        break;
                    }

                    Status($"Gathering Event Logs ({_machine})");
                    //get event log
                    EventRecord[] _records = GetSystemLogs(_machine, _lockedOut.SamAccountName, _cancel);
                    if (_records.Count() > 0 && !_cancel.IsCancellationRequested)
                    {
                        Status($"Processing Event Logs ({_machine})");
                        SafeSetProgressBarMax(progressBar.Maximum + _records.Count());
                        SafeBeginUpdate(lstEvents);
                        foreach (EventRecord _rec in _records)
                        {
                            if (_cancel.IsCancellationRequested)
                            {
                                break;
                            }

                            string[] _eventItemValues = new string[4];
                            _eventItemValues[0] = _rec.TimeCreated.ToString();
                            _eventItemValues[1] = _rec.MachineName;
                            string szEventType = "UNKNOWN";
                            string szEventReason = "UNKNOWN";
                            if (_EventTypeLookup.ContainsKey(_rec.Id))
                            {
                                szEventType = _EventTypeLookup[_rec.Id];
                                if (szEventType.EndsWith("(2K3)")) // old events
                                {
                                    szEventReason = szEventType.Split(new char[] { '(', ')' })[1];
                                    szEventType = szEventType.Split(new char[] { '(', ')' })[0].TrimEnd(' ');
                                }
                                else if (_rec.Id == 4625)
                                {
                                    if (_StatusLookup.ContainsKey(Convert.ToInt32(_rec.Properties[7].Value)))
                                    {
                                        szEventReason = _StatusLookup[Convert.ToInt32(_rec.Properties[7].Value)];
                                    }
                                }
                                else if (_rec.Id == 4624)
                                {
                                    if (_StatusLookup.ContainsKey(Convert.ToInt32(_rec.Properties[8].Value)))
                                    {
                                        szEventReason = _StatusLookup[Convert.ToInt32(_rec.Properties[8].Value)];
                                    }
                                }
                            }
                            _eventItemValues[2] = szEventType;
                            _eventItemValues[3] = szEventReason;
                            ListViewItem _eventItem = new ListViewItem(_eventItemValues)
                            {
                                Tag = _rec,
                                ToolTipText = _rec.ToXml()
                            };
                            SafeAddItem(lstEvents, _eventItem);
                            SafeSetProgressBarValue(progressBar.Value + 1);
                        }
                        SafeEndUpdate(lstEvents);
                        Status($"Processed Event Logs ({_machine})");
                        if (_cancel.IsCancellationRequested)
                        {
                            break;
                        }

                        System.Threading.Thread.Sleep(1000);
                    }
                    else
                    {
                        Status($"No Matching Event Logs ({_machine})");
                        if (_cancel.IsCancellationRequested)
                        {
                            break;
                        }

                        System.Threading.Thread.Sleep(1000);
                    }
                    if (_cancel.IsCancellationRequested)
                    {
                        break;
                    }

                    Status($"Gathering Service Accounts on ({_machine})");
                    CimInstance[] _Services = GetServices(_machine, _lockedOut.SamAccountName, _cancel);
                    if (_Services.Count() > 0 && !_cancel.IsCancellationRequested)
                    {
                        Status($"Processing Service Accounts ({_machine})");
                        SafeSetProgressBarMax(progressBar.Maximum + _Services.Count());
                        SafeBeginUpdate(lstServices);
                        foreach (CimInstance _svc in _Services)
                        {
                            if (_cancel.IsCancellationRequested)
                            {
                                break;
                            }

                            string[] _eventItemValues = new string[4];
                            _eventItemValues[0] = _machine;
                            _eventItemValues[1] = _svc.CimInstanceProperties["Name"].Value.ToString();
                            _eventItemValues[2] = _svc.CimInstanceProperties["ProcessID"].Value.ToString();
                            _eventItemValues[3] = _svc.CimInstanceProperties["State"].Value.ToString();
                            ListViewItem _newItem = new ListViewItem(_eventItemValues)
                            {
                                Tag = _svc
                            };
                            SafeAddItem(lstServices, _newItem);
                            SafeSetProgressBarValue(progressBar.Value + 1);
                        }
                        SafeEndUpdate(lstServices);
                        Status($"Processed Service Accounts ({_machine})");
                        if (_cancel.IsCancellationRequested)
                        {
                            break;
                        }

                        System.Threading.Thread.Sleep(1000);
                    }
                    else
                    {
                        Status($"No Matching Service Accounts ({_machine})");
                        if (_cancel.IsCancellationRequested)
                        {
                            break;
                        }

                        System.Threading.Thread.Sleep(1000);
                    }
                    if (_cancel.IsCancellationRequested)
                    {
                        break;
                    }

                    Status($"Gathering Logged On Users ({_machine})");
                    ITerminalServicesSession[] _Users = GetUsers(_machine, _lockedOut.SamAccountName, _cancel);
                    if (_Users.Count() > 0 && !_cancel.IsCancellationRequested)
                    {
                        Status($"Processing Logged On Users ({_machine})");
                        SafeSetProgressBarMax(progressBar.Maximum + _Users.Count());
                        SafeBeginUpdate(lstSignedOn);
                        foreach (ITerminalServicesSession _usr in _Users)
                        {
                            if (_cancel.IsCancellationRequested)
                            {
                                break;
                            }

                            string[] _eventItemValues = new string[4];
                            _eventItemValues[0] = _machine;
                            _eventItemValues[1] = _usr.LoginTime.ToString();
                            _eventItemValues[2] = _usr.SessionId.ToString();
                            _eventItemValues[3] = _usr.ConnectionState.ToString();
                            ListViewItem _newItem = new ListViewItem(_eventItemValues)
                            {
                                Tag = _usr
                            };
                            SafeAddItem(lstSignedOn, _newItem);
                            SafeSetProgressBarValue(progressBar.Value + 1);
                        }
                        SafeEndUpdate(lstSignedOn);
                        Status($"Processed Logged On Users ({_machine})");
                        if (_cancel.IsCancellationRequested)
                        {
                            break;
                        }

                        System.Threading.Thread.Sleep(1000);
                    }
                    else
                    {
                        Status($"No Matching Logged On Users ({_machine})");
                        if (_cancel.IsCancellationRequested)
                        {
                            break;
                        }

                        System.Threading.Thread.Sleep(1000);
                    }
                    //get services
                    SafeSetProgressBarValue(progressBar.Value + 1);
                }
                if (!_cancel.IsCancellationRequested)
                {
                    SafeSort(lstEvents);
                    SafeSort(lstServices);
                    SafeSort(lstSignedOn);
                    TimeSpan _taskLength = DateTime.UtcNow - _taskStart;
                    Status($"Analysis completed in {_taskLength.ToString()}");
                }
                else if (_cancel.IsCancellationRequested)
                {
                    Status($"Analysis cancelled");
                }
                SafeSetButtonText(btnRunAnalysis, "Run Analysis");
                SafeUpdateUI(true);
            }
        }

        private ITerminalServicesSession[] GetUsers(string szMachine, string szUsername, CancellationToken _cancel)
        {
            bool _exceptionThrown = false;
            //List<CimInstance> _results = new List<CimInstance>();
            List<ITerminalServicesSession> _results = new List<ITerminalServicesSession>();
            if (!_cancel.IsCancellationRequested)
            {
                try
                {
                    ITerminalServicesManager _tManager = new TerminalServicesManager();

                    using (ITerminalServer _tServer = _tManager.GetRemoteServer(szMachine))
                    {
                        _tServer.Open();
                        foreach (ITerminalServicesSession _sesh in _tServer.GetSessions())
                        {
                            if (_cancel.IsCancellationRequested)
                            {
                                break;
                            }

                            if (_sesh.UserName.ToLower().Contains(szUsername.ToLower()))
                            {
                                _results.Add(_sesh);
                            }
                        }
                    }
                }
                catch (Win32Exception _w32)
                {
                    switch (_w32.HResult)
                    {
                        case unchecked((int)0x80004005):
                            Status($"Error Gathering Logged On Users ({szMachine}) - most likely offline / firewall");
                            break;
                        default:
                            if (System.Diagnostics.Debugger.IsAttached)
                            {
                                System.Diagnostics.Debugger.Break();
                            }

                            Status($"Error Gathering Logged On Users ({szMachine}) - PROVIDE TO SUPPORT - W32 UNKNKOWN ({_w32.HResult})");
                            break;
                    }
                    _exceptionThrown = true;
                }
                catch (Exception _ex)
                {
                    if (System.Diagnostics.Debugger.IsAttached)
                    {
                        System.Diagnostics.Debugger.Break();
                    }

                    Status($"Error Gathering Logged On Users ({szMachine}) - PROVIDE TO SUPPORT - UNKNKOWN ({_ex.GetType().Name}:{_ex.HResult})");
                    _exceptionThrown = true;
                }
                if (_exceptionThrown)
                {
                    _results.Clear(); // clear results so we do not error and can continue.
                    System.Threading.Thread.Sleep(5000); // give them time to see the error
                }
            }
            return _results.ToArray();
        }

        private CimInstance[] GetServices(string szMachine, string szUsername, CancellationToken _cancel)
        {
            bool _exceptionThrown = false;
            List<CimInstance> _results = new List<CimInstance>();
            if (!_cancel.IsCancellationRequested)
            {
                try
                {
                    string Namespace = @"root\cimv2";
                    string OSQuery = "SELECT * FROM Win32_Service";
                    CimSession mySession = CimSession.Create(szMachine);
                    IEnumerable<CimInstance> queryInstance = mySession.QueryInstances(Namespace, "WQL", OSQuery);
                    foreach (CimInstance _svc in queryInstance)
                    {
                        if (_cancel.IsCancellationRequested)
                        {
                            break;
                        }

                        string szServiceName = _svc.CimInstanceProperties["StartName"].Value as string;
                        if (!string.IsNullOrWhiteSpace(szServiceName))
                        {
                            if (szServiceName.ToLower().Contains(szUsername.ToLower()))
                            {
                                _results.Add(_svc);
                            }
                        }
                    }
                }
                catch (CimException _cex)
                {
                    switch (_cex.HResult)
                    {
                        case unchecked((int)0x80338126):
                        case unchecked((int)0x80131500):
                            Status($"Error Gathering Service Accounts ({szMachine}) - most likely offline / firewall");
                            break;
                        case unchecked((int)0x80070005):
                            Status($"Error Gathering Service Accounts ({szMachine}) - invalid permissions");
                            break;
                        default:
                            if (System.Diagnostics.Debugger.IsAttached)
                            {
                                System.Diagnostics.Debugger.Break();
                            }

                            Status($"Error Gathering Service Accounts ({szMachine}) - PROVIDE TO SUPPORT - CIM UNKNKOWN ({_cex.HResult})");
                            break;
                    }
                    _exceptionThrown = true;
                }
                catch (Exception _ex)
                {
                    if (System.Diagnostics.Debugger.IsAttached)
                    {
                        System.Diagnostics.Debugger.Break();
                    }

                    Status($"Error Gathering Service Accounts ({szMachine}) - PROVIDE TO SUPPORT - UNKNKOWN ({_ex.GetType().Name}:{_ex.HResult})");
                    _exceptionThrown = true;
                }
                if (_exceptionThrown)
                {
                    _results.Clear(); // clear results so we do not error and can continue.
                    System.Threading.Thread.Sleep(5000); // give them time to see the error
                }
            }
            return _results.ToArray();
        }

        private EventRecord[] GetSystemLogs(string szMachine, string szUsername, CancellationToken _cancel)
        {
            bool _exceptionThrown = false;
            List<EventRecord> _results = new List<EventRecord>();//4740, 644, 6279
                                                                 //DateTime.UtcNow.ToString("o")
                                                                 //*[System[TimeCreated[@SystemTime&gt;='2019-05-28T20:39:29.000Z' and @SystemTime&lt;='2019-05-29T20:39:29.999Z']]]
            string queryDSL = $"<QueryList>\r\n" +
                              $"  <Query Id=\"0\" Path=\"Security\">\r\n" +
                              $"    <Select Path=\"Security\">*[System[( (EventID &gt;= 4624 and EventID &lt;= 4625)  or EventID=4740 or EventID=6279 or  (EventID &gt;= 528 and EventID &lt;= 529)  or EventID=644) and TimeCreated[@SystemTime&gt;='{dateTimeStart.Value.ToUniversalTime().ToString("o")}' and @SystemTime&lt;='{dateTimeEnd.Value.ToUniversalTime().ToString("o")}']]]</Select>\r\n" +
                              $"  </Query>\r\n" +
                              $"</QueryList>\r\n";
            if (!string.IsNullOrWhiteSpace(szMachine) && !_cancel.IsCancellationRequested)
            {
                try
                {
                    EventLogQuery eventsQuery = new EventLogQuery("Security", PathType.LogName, queryDSL)
                    {
                        Session = new EventLogSession(szMachine)
                    };
                    EventLogReader logReader = new EventLogReader(eventsQuery);
                    for (EventRecord eventdetail = logReader.ReadEvent(); eventdetail != null; eventdetail = logReader.ReadEvent())
                    {
                        if (_cancel.IsCancellationRequested)
                        {
                            break;
                        }

                        bool _Found = false;
                        foreach (EventProperty _prop in eventdetail.Properties)
                        {
                            if (_cancel.IsCancellationRequested)
                            {
                                break;
                            }

                            if (_prop.Value.ToString().ToLower().Contains(szUsername))
                            {
                                _results.Add(eventdetail);
                                _Found = true;
                                break;
                            }
                        }
                        if (!_Found)
                        {
                            EventRecord test = eventdetail;
                        }
                    }
                }
                catch (EventLogException _evx)
                {
                    switch (_evx.HResult)
                    {
                        case unchecked((int)0x80131500):
                            Status($"Error Gathering Event Logs ({szMachine}) - most likely offline / firewall");
                            break;
                        default:
                            if (System.Diagnostics.Debugger.IsAttached)
                            {
                                System.Diagnostics.Debugger.Break();
                            }

                            Status($"Error Gathering Event Logs ({szMachine}) - PROVIDE TO SUPPORT - W32 UNKNKOWN ({_evx.HResult})");
                            break;
                    }
                    _exceptionThrown = true;
                }
                catch (UnauthorizedAccessException)
                {
                    Status($"Error Gathering Event Logs ({szMachine}) - unauthorized");
                    _exceptionThrown = true;
                }
                catch (Exception _ex)
                {
                    if (System.Diagnostics.Debugger.IsAttached)
                    {
                        System.Diagnostics.Debugger.Break();
                    }

                    Status($"Error Gathering Event Logs ({szMachine}) - PROVIDE TO SUPPORT - UNKNKOWN ({_ex.GetType().Name}:{_ex.HResult})");
                    _exceptionThrown = true;
                }
                if (_exceptionThrown)
                {
                    _results.Clear(); // clear results so we do not error and can continue.
                    System.Threading.Thread.Sleep(5000); // give them time to see the error
                }
            }
            return _results.ToArray();
        }

        private bool groupBoxesInvokeRequired()
        {
            return (groupBox1.InvokeRequired || groupBox2.InvokeRequired || groupBox3.InvokeRequired || groupBox4.InvokeRequired || groupBox5.InvokeRequired || groupBox6.InvokeRequired || groupBox7.InvokeRequired);
        }
        private bool ViewsInvokeRequired()
        {
            return (viewMachines.InvokeRequired || lstEvents.InvokeRequired || lstServices.InvokeRequired || lstSignedOn.InvokeRequired);
        }
        private bool LabelsInvokeRequired()
        {
            return (lblEnabled.InvokeRequired || lblLocked.InvokeRequired || lblLockoutTime.InvokeRequired || lblBadLogonCount.InvokeRequired || lblExpirationDate.InvokeRequired || lblLastBadPassword.InvokeRequired || lblLastLogon.InvokeRequired || lblLastPasswordSet.InvokeRequired);
        }
        private bool DateTimePickersInvokeRequired()
        {
            return (dateTimeStart.InvokeRequired || dateTimeEnd.InvokeRequired);
        }
        private bool ButtonsInvokeRequired()
        {
            return (btnRunAnalysis.InvokeRequired);
        }
        private delegate void SafeCallDelegateUpdateUI(bool Enabled);
        private void SafeUpdateUI(bool Enabled)
        {
            if (groupBoxesInvokeRequired() || ViewsInvokeRequired() || LabelsInvokeRequired() || DateTimePickersInvokeRequired() || ButtonsInvokeRequired())
            {
                SafeCallDelegateUpdateUI d = new SafeCallDelegateUpdateUI(SafeUpdateUI);
                Invoke(d, new object[] { Enabled });
            }
            else
            {
                UpdateUI();
                groupBox1.Enabled = Enabled;
                groupBox2.Enabled = Enabled;
                groupBox3.Enabled = Enabled;
                groupBox4.Enabled = Enabled;
                groupBox5.Enabled = Enabled;
                groupBox6.Enabled = Enabled;
                lstEvents.Enabled = Enabled;
                lstServices.Enabled = Enabled;
                lstSignedOn.Enabled = Enabled;
                dateTimeStart.Enabled = Enabled;
                dateTimeEnd.Enabled = Enabled;
                if (Enabled == false)
                {
                    lblEnabled.Text = "Needs analysis";
                    lblLocked.Text = "Needs analysis";
                    lblLockoutTime.Text = "Needs analysis";
                    lblBadLogonCount.Text = "Needs analysis";
                    lblExpirationDate.Text = "Needs analysis";
                    lblLastBadPassword.Text = "Needs analysis";
                    lblLastLogon.Text = "Needs analysis";
                    lblLastPasswordSet.Text = "Needs analysis";
                }
                Update();
            }
        }

        private string[] SelectedMachines(TreeNodeCollection _Nodes)
        {
            List<string> _MachineResults = new List<string>();
            foreach (TreeNode _check in _Nodes)
            {
                if (_check.Checked && _check.Tag != null)
                {
                    _MachineResults.Add(_check.Text);
                }
                if (_check.Nodes.Count > 0)
                {
                    _MachineResults.AddRange(SelectedMachines(_check.Nodes));
                }
            }
            return _MachineResults.ToArray();
        }

        private UserPrincipal FindUserPrincipal(Domain enumDomain, string szUsername)
        {
            UserPrincipal _result = null;
            try
            {
                PrincipalContext _pContext = new PrincipalContext(ContextType.Domain, enumDomain.Name);
                _result = UserPrincipal.FindByIdentity(_pContext, szUsername);
            }
            catch (Exception _adEX)
            {
                _adEX = _adEX;
            }
            return _result;
        }

        private void lblEnabled_TextChanged(object sender, EventArgs e)
        {
            if (lblEnabled.Text.ToUpper() == "TRUE")
            {
                lblEnabled.ForeColor = Color.Green;
            }
            else if (lblEnabled.Text.ToUpper() == "FALSE")
            {
                lblEnabled.ForeColor = Color.Red;
            }

            lblEnabled.Invalidate();
        }

        private void lblLocked_TextChanged(object sender, EventArgs e)
        {
            if (lblLocked.Text.ToUpper() == "TRUE")
            {
                lblLocked.ForeColor = Color.Green;
            }
            else if (lblLocked.Text.ToUpper() == "FALSE")
            {
                lblLocked.ForeColor = Color.Red;
            }

            lblLocked.Invalidate();
        }

        private void lstEvents_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (lstEvents.SelectedItems.Count == 1)
                {
                    Clipboard.Clear();
                    Clipboard.SetText((lstEvents.SelectedItems[0].Tag as EventRecord).ToXml());
                }
            }
        }
    }
}
