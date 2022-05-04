namespace Simpl_Sharp_Pro_Template;
        // class declarations
         class TcpConnection;
     class TcpConnection 
    {
        // class delegates

        // class events
        EventHandler DataReceived ( TcpConnection sender, EventArgs e );

        // class functions
        FUNCTION Connect ( STRING hostname , SIGNED_LONG_INTEGER port );
        FUNCTION Write ( STRING message );
        SIGNED_LONG_INTEGER_FUNCTION GetHashCode ();
        STRING_FUNCTION ToString ();

        // class variables
        INTEGER __class_id__;

        // class properties
        STRING ResponseString[];
    };

