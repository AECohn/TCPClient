namespace TCPClient;
        // class declarations
         class SendArgs;
         class TcpConnection;
     class SendArgs 
    {
        // class delegates

        // class events

        // class functions
        SIGNED_LONG_INTEGER_FUNCTION GetHashCode ();
        STRING_FUNCTION ToString ();

        // class variables
        STRING Data[];

        // class properties
    };

     class TcpConnection 
    {
        // class delegates

        // class events
        EventHandler DataReceived ( TcpConnection sender, SendArgs e );
        EventHandler ConnectionStatus ( TcpConnection sender, SendArgs e );

        // class functions
        FUNCTION Connect ( STRING hostname , INTEGER port );
        FUNCTION Write ( STRING message );
        FUNCTION Disconnect ();
        SIGNED_LONG_INTEGER_FUNCTION GetHashCode ();
        STRING_FUNCTION ToString ();

        // class variables
        INTEGER __class_id__;

        // class properties
    };

