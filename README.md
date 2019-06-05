# ADLockAudit
Utility to help analyze active directory lock outs, only tested on 2008R2-2016 server's/domain controllers. 
Depends on local admin rights, domain admin rights, proper ad audit policy, windows remoting and WMI. 

Basically simplifies steps I personally would take to anlyze lockouts.

Enumerates domain controllers and all machines via active directory.

On analysis pulls current active directory information for the account specified if it is found. 

It will then scan all selected machines remotely to the best of it's ability. This is highly dependent on internal GPO policies
for auditing, windows/wmi remoting, and firewall policies. If you can't connect to the event log, service manager, etc remotely,
the likely hood is this tool will fail.

1. Pulls event logs auditing for known account login failures/successes/lockouts and creates a timeline of events sorted by date.
2. Pulls all services and audits the service accounts for any account matching the user and reports the machine, service name, id and state.
3. Pulls all sessions and audits them for any sessions matching the user and report, machine, id, and state.

This information can then be used to help track down possible causes of AD account lockout.
