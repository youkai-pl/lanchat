//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Lanchat.Terminal.Properties {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    ///   This class was generated by MSBuild using the GenerateResource task.
    ///   To add or remove a member, edit your .resx file then rerun MSBuild.
    /// </summary>
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Build.Tasks.StronglyTypedResourceBuilder", "15.1.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Lanchat.Terminal.Properties.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Aliases.
        /// </summary>
        internal static string Aliases {
            get {
                return ResourceManager.GetString("Aliases", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Address already blocked.
        /// </summary>
        internal static string AlreadyBlocked {
            get {
                return ResourceManager.GetString("AlreadyBlocked", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to {0} blocked.
        /// </summary>
        internal static string Blocked {
            get {
                return ResourceManager.GetString("Blocked", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Blocked IP addresses.
        /// </summary>
        internal static string BlockedList {
            get {
                return ResourceManager.GetString("BlockedList", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Cannot access {0}
        ///File name or permissions may be wrong..
        /// </summary>
        internal static string CannotAccessFile {
            get {
                return ResourceManager.GetString("CannotAccessFile", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Cannot connect with {0}.
        /// </summary>
        internal static string CannotConnect {
            get {
                return ResourceManager.GetString("CannotConnect", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Command not found.
        /// </summary>
        internal static string CommandNotFound {
            get {
                return ResourceManager.GetString("CommandNotFound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to {0} connected.
        /// </summary>
        internal static string Connected {
            get {
                return ResourceManager.GetString("Connected", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Attempting connect to {0}.
        /// </summary>
        internal static string ConnectionAttempt {
            get {
                return ResourceManager.GetString("ConnectionAttempt", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to {0} disconnected.
        /// </summary>
        internal static string Disconnected {
            get {
                return ResourceManager.GetString("Disconnected", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Cancelled.
        /// </summary>
        internal static string FileTransferCancelled {
            get {
                return ResourceManager.GetString("FileTransferCancelled", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Finished.
        /// </summary>
        internal static string FileTransferFinished {
            get {
                return ResourceManager.GetString("FileTransferFinished", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Cannot send multiple files at once..
        /// </summary>
        internal static string FileTransferInProgress {
            get {
                return ResourceManager.GetString("FileTransferInProgress", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Rejected.
        /// </summary>
        internal static string FileTransferRejected {
            get {
                return ResourceManager.GetString("FileTransferRejected", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to File transfer.
        /// </summary>
        internal static string FileTransferTab {
            get {
                return ResourceManager.GetString("FileTransferTab", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Waiting for accept.
        /// </summary>
        internal static string FileTransferWaiting {
            get {
                return ResourceManager.GetString("FileTransferWaiting", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to To make sure the connection with {0} is secure check fingerprint in &quot;Users&quot; tab..
        /// </summary>
        internal static string FreshRsa {
            get {
                return ResourceManager.GetString("FreshRsa", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Check /help command.
        /// </summary>
        internal static string GetHelp {
            get {
                return ResourceManager.GetString("GetHelp", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 
        ///Lanchat: LAN chat and file transfer
        ///https://youkai.pl/lanchat-docs
        ///
        ///Tab / Shift+Tab       - tabs switching
        ///Up Arrow / Down Arrow - input history
        ///PageUp / PageDown     - scrolling
        ///
        ///Commands:
        ///connect   disconnect   nick    
        ///block     unblock      blocked
        ///afk       dnd          online
        ///send      accept       reject
        ///cancel    exit         help
        ///
        ///Try /help [command] for more info.
        ///      
        ///Config file can be found in:
        ///{0}
        ///        .
        /// </summary>
        internal static string Help {
            get {
                return ResourceManager.GetString("Help", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Incorrect command usage
        ///Check /help [command].
        /// </summary>
        internal static string IncorrectCommandUsage {
            get {
                return ResourceManager.GetString("IncorrectCommandUsage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to     
        ///     __                     __          __
        ///    / /   ____ _____  _____/ /_  ____ _/ /_
        ///   / /   / __ `/ __ \/ ___/ __ \/ __ `/ __/
        ///  / /___/ /_/ / / / / /__/ / / / /_/ / /_
        /// /_____/\__,_/_/ /_/\___/_/ /_/\__,_/\__/
        ///              {0}.
        /// </summary>
        internal static string Logo {
            get {
                return ResourceManager.GetString("Logo", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Lanchat.
        /// </summary>
        internal static string MainTab {
            get {
                return ResourceManager.GetString("MainTab", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to {0} changed nick to {1}.
        /// </summary>
        internal static string NicknameChanged {
            get {
                return ResourceManager.GetString("NicknameChanged", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to No file transfers in progress..
        /// </summary>
        internal static string NoFileReceiveRequest {
            get {
                return ResourceManager.GetString("NoFileReceiveRequest", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Port busy. Cannot start server..
        /// </summary>
        internal static string PortBusy {
            get {
                return ResourceManager.GetString("PortBusy", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Public key of {0} has changed. Connection may be not secure..
        /// </summary>
        internal static string RsaChanged {
            get {
                return ResourceManager.GetString("RsaChanged", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Nickname changed to {0}.
        /// </summary>
        internal static string SelfNicknameChanged {
            get {
                return ResourceManager.GetString("SelfNicknameChanged", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Summary.
        /// </summary>
        internal static string Summary {
            get {
                return ResourceManager.GetString("Summary", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 
        ///    Accept incoming file transfer request.
        ///        .
        /// </summary>
        internal static string Summary_accept {
            get {
                return ResourceManager.GetString("Summary_accept", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 
        ///    Change status to &quot;Away From Keyboard&quot;
        ///        .
        /// </summary>
        internal static string Summary_afk {
            get {
                return ResourceManager.GetString("Summary_afk", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 
        ///    Prevent from connecting to specified user.
        ///    If you are not currently connected you can block by IP address.
        ///        .
        /// </summary>
        internal static string Summary_block {
            get {
                return ResourceManager.GetString("Summary_block", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 
        ///    Show list of blocked IP addresses.
        ///        .
        /// </summary>
        internal static string Summary_blocked {
            get {
                return ResourceManager.GetString("Summary_blocked", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 
        ///    Cancel file transfer.
        ///        .
        /// </summary>
        internal static string Summary_cancel {
            get {
                return ResourceManager.GetString("Summary_cancel", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 
        ///    Connect with other users.
        ///        .
        /// </summary>
        internal static string Summary_connect {
            get {
                return ResourceManager.GetString("Summary_connect", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 
        ///    Change status to &quot;Do Not Disturb&quot;
        ///        .
        /// </summary>
        internal static string Summary_dnd {
            get {
                return ResourceManager.GetString("Summary_dnd", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 
        ///    Changes user nickname.
        ///    Nickname have to be between 1-20 characters.
        ///    Nickname cannot contains spaces.
        ///        .
        /// </summary>
        internal static string Summary_nick {
            get {
                return ResourceManager.GetString("Summary_nick", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 
        ///    Change status to &quot;Online&quot;
        ///        .
        /// </summary>
        internal static string Summary_online {
            get {
                return ResourceManager.GetString("Summary_online", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 
        ///    Reject incoming file transfer request.
        ///        .
        /// </summary>
        internal static string Summary_reject {
            get {
                return ResourceManager.GetString("Summary_reject", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 
        ///    Send file. Make sure path in argument doesn&apos;t have quotation marks.
        ///    If the target user accepts the request, the transfer will begin.
        ///        .
        /// </summary>
        internal static string Summary_send {
            get {
                return ResourceManager.GetString("Summary_send", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 
        ///    Unblock user. You have to user IP address in argument.
        ///    Check /blocked command for blocked addresses list.
        ///        .
        /// </summary>
        internal static string Summary_unblock {
            get {
                return ResourceManager.GetString("Summary_unblock", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Syntax.
        /// </summary>
        internal static string Syntax {
            get {
                return ResourceManager.GetString("Syntax", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to  
        ///    /accept [ID]
        ///        .
        /// </summary>
        internal static string Syntax_accept {
            get {
                return ResourceManager.GetString("Syntax_accept", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 
        ///    /block [ID]
        ///    /block [IP address]
        ///        .
        /// </summary>
        internal static string Syntax_block {
            get {
                return ResourceManager.GetString("Syntax_block", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 
        ///    /cancel [ID]
        ///        .
        /// </summary>
        internal static string Syntax_cancel {
            get {
                return ResourceManager.GetString("Syntax_cancel", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 
        ///    /connect [ip]
        ///    /connect [ip] [port]
        ///    /connect [domain]
        ///    /connect [domain] [port]
        ///        .
        /// </summary>
        internal static string Syntax_connect {
            get {
                return ResourceManager.GetString("Syntax_connect", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 
        ///    /disconnect [ID]
        ///        .
        /// </summary>
        internal static string Syntax_disconnect {
            get {
                return ResourceManager.GetString("Syntax_disconnect", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 
        ///    /nick [new nick]
        ///        .
        /// </summary>
        internal static string Syntax_nick {
            get {
                return ResourceManager.GetString("Syntax_nick", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 
        ///    /reject [ID]
        ///        .
        /// </summary>
        internal static string Syntax_reject {
            get {
                return ResourceManager.GetString("Syntax_reject", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 
        ///    /send [ID] [file path]
        ///        .
        /// </summary>
        internal static string Syntax_send {
            get {
                return ResourceManager.GetString("Syntax_send", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 
        ///    /unblock [IP address]
        ///        .
        /// </summary>
        internal static string Syntax_unblock {
            get {
                return ResourceManager.GetString("Syntax_unblock", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to {0} unblocked.
        /// </summary>
        internal static string Unblocked {
            get {
                return ResourceManager.GetString("Unblocked", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to User not found.
        /// </summary>
        internal static string UserNotFound {
            get {
                return ResourceManager.GetString("UserNotFound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Users.
        /// </summary>
        internal static string UsersTab {
            get {
                return ResourceManager.GetString("UsersTab", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Lanchat.
        /// </summary>
        internal static string WindowTitle {
            get {
                return ResourceManager.GetString("WindowTitle", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Nickname must be less than 20 characters long and must not contain spaces..
        /// </summary>
        internal static string WrongNickname {
            get {
                return ResourceManager.GetString("WrongNickname", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Your key fingerprint:.
        /// </summary>
        internal static string YourRsa {
            get {
                return ResourceManager.GetString("YourRsa", resourceCulture);
            }
        }
    }
}
