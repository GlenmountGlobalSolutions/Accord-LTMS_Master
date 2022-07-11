Imports System
Imports System.Runtime.ConstrainedExecution
Imports System.Runtime.InteropServices
Imports System.Security
Imports System.Security.Permissions
Imports System.Security.Principal
Imports Microsoft.Win32.SafeHandles


<PermissionSet(SecurityAction.Demand, Name:="FullTrust")> _
Public Class Impersonation
    Implements IDisposable

    Private ReadOnly _handle As SafeTokenHandle
    Private ReadOnly _context As WindowsImpersonationContext

    Public Enum LogonType As Integer
        'This logon type is intended for users who will be interactively using the computer, such as a user being logged on
        'by a terminal server, remote shell, or similar process.
        'This logon type has the additional expense of caching logon information for disconnected operations;
        'therefore, it is inappropriate for some client/server applications,
        'such as a mail server.
        LOGON32_LOGON_INTERACTIVE = 2

        'This logon type is intended for high performance servers to authenticate plaintext passwords.
        'The LogonUser function does not cache credentials for this logon type.
        LOGON32_LOGON_NETWORK = 3

        'This logon type is intended for batch servers, where processes may be executing on behalf of a user without
        'their direct intervention. This type is also for higher performance servers that process many plaintext
        'authentication attempts at a time, such as mail or Web servers.
        'The LogonUser function does not cache credentials for this logon type.
        LOGON32_LOGON_BATCH = 4

        'Indicates a service-type logon. The account provided must have the service privilege enabled.
        LOGON32_LOGON_SERVICE = 5

        'This logon type is for GINA DLLs that log on users who will be interactively using the computer.
        'This logon type can generate a unique audit record that shows when the workstation was unlocked.
        LOGON32_LOGON_UNLOCK = 7

        'This logon type preserves the name and password in the authentication package, which allows the server to make
        'connections to other network servers while impersonating the client. A server can accept plaintext credentials
        'from a client, call LogonUser, verify that the user can access the system across the network, and still
        'communicate with other servers.
        'NOTE: Windows NT:  This value is not supported.
        LOGON32_LOGON_NETWORK_CLEARTEXT = 8

        'This logon type allows the caller to clone its current token and specify new credentials for outbound connections.
        'The new logon session has the same local identifier but uses different credentials for other network connections.
        'NOTE: This logon type is supported only by the LOGON32_PROVIDER_WINNT50 logon provider.
        'NOTE: Windows NT:  This value is not supported.
        LOGON32_LOGON_NEW_CREDENTIALS = 9
    End Enum


    Public Sub New(ByVal domain As String, ByVal username As String, ByVal password As String)
        Dim ok = LogonUser(username, domain, password, LogonType.LOGON32_LOGON_NEW_CREDENTIALS, 0, Me._handle)
        If (Not ok) Then
            Dim errorCode = Marshal.GetLastWin32Error()
            Throw New ApplicationException(String.Format("Could not impersonate the elevated user.  LogonUser returned error code {0}.", errorCode))
        End If

        Me._context = WindowsIdentity.Impersonate(Me._handle.DangerousGetHandle())
    End Sub

    Public Sub Dispose() Implements IDisposable.Dispose
        Me._context.Dispose()
        Me._handle.Dispose()
    End Sub

    <DllImport("advapi32.dll", SetLastError:=True, CharSet:=CharSet.Unicode)> _
    Private Shared Function LogonUser(ByVal lpszUsername As String, ByVal lpszDomain As String, ByVal lpszPassword As String, ByVal dwLogonType As Integer, ByVal dwLogonProvider As Integer, <System.Runtime.InteropServices.Out()> ByRef phToken As SafeTokenHandle) As Boolean
    End Function

    Public NotInheritable Class SafeTokenHandle
        Inherits SafeHandleZeroOrMinusOneIsInvalid
        Private Sub New()
            MyBase.New(True)
        End Sub

        <DllImport("kernel32.dll"), ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success), SuppressUnmanagedCodeSecurity()> _
        Private Shared Function CloseHandle(ByVal handle As IntPtr) As <MarshalAs(UnmanagedType.Bool)> Boolean
        End Function

        Protected Overrides Function ReleaseHandle() As Boolean
            Return CloseHandle(handle)
        End Function
    End Class

End Class
