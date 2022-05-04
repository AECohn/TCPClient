namespace Simpl_Sharp_Pro_Template;
        // class declarations
         class ResponseArgs;
         class TcpConnection;
     class ResponseArgs 
    {
        // class delegates

        // class events

        // class functions
        SIGNED_LONG_INTEGER_FUNCTION GetHashCode ();
        STRING_FUNCTION ToString ();

        // class variables
        SIMPLSHARPSTRING Response[];

        // class properties
    };

     class TcpConnection 
    {
        // class delegates

        // class events
        EventHandler DataReceived ( TcpConnection sender, ResponseArgs e );

        // class functions
        FUNCTION Connect ( STRING hostname , INTEGER port );
        FUNCTION Write ( STRING message );
        FUNCTION Disconnect ();
        SIGNED_LONG_INTEGER_FUNCTION GetHashCode ();
        STRING_FUNCTION ToString ();

        // class variables
        INTEGER __class_id__;

        // class properties
        STRING ResponseString[];
    };

