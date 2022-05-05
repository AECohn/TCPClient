using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Linq;
using Crestron;
using Crestron.Logos.SplusLibrary;
using Crestron.Logos.SplusObjects;
using Crestron.SimplSharp;
using TCPClient;
using Crestron.SimplSharp.Python;

namespace UserModule_TCP_CLIENT_SHARP
{
    public class UserModuleClass_TCP_CLIENT_SHARP : SplusObject
    {
        static CCriticalSection g_criticalSection = new CCriticalSection();
        
        Crestron.Logos.SplusObjects.StringInput ADDRESS;
        Crestron.Logos.SplusObjects.StringInput TX;
        Crestron.Logos.SplusObjects.StringInput QUEUEMILLISECONDS;
        Crestron.Logos.SplusObjects.StringOutput RX;
        Crestron.Logos.SplusObjects.AnalogInput PORTNUMBER;
        Crestron.Logos.SplusObjects.DigitalInput CONNECT;
        Crestron.Logos.SplusObjects.DigitalInput ENABLEQUEUE;
        Crestron.Logos.SplusObjects.DigitalOutput CONNECTED;
        TCPClient.TcpConnection MYCONNECTION;
        object CONNECT_OnPush_0 ( Object __EventInfo__ )
        
            { 
            Crestron.Logos.SplusObjects.SignalEventArgs __SignalEventArg__ = (Crestron.Logos.SplusObjects.SignalEventArgs)__EventInfo__;
            try
            {
                SplusExecutionContext __context__ = SplusThreadStartCode(__SignalEventArg__);
                
                __context__.SourceCodeLine = 23;
                MYCONNECTION . Connect ( ADDRESS .ToString(), (ushort)( PORTNUMBER  .UshortValue )) ; 
                
                
            }
            catch(Exception e) { ObjectCatchHandler(e); }
            finally { ObjectFinallyHandler( __SignalEventArg__ ); }
            return this;
            
        }
        
    object CONNECT_OnRelease_1 ( Object __EventInfo__ )
    
        { 
        Crestron.Logos.SplusObjects.SignalEventArgs __SignalEventArg__ = (Crestron.Logos.SplusObjects.SignalEventArgs)__EventInfo__;
        try
        {
            SplusExecutionContext __context__ = SplusThreadStartCode(__SignalEventArg__);
            
            __context__.SourceCodeLine = 28;
            MYCONNECTION . Disconnect ( ) ; 
            
            
        }
        catch(Exception e) { ObjectCatchHandler(e); }
        finally { ObjectFinallyHandler( __SignalEventArg__ ); }
        return this;
        
    }
    
object TX_OnChange_2 ( Object __EventInfo__ )

    { 
    Crestron.Logos.SplusObjects.SignalEventArgs __SignalEventArg__ = (Crestron.Logos.SplusObjects.SignalEventArgs)__EventInfo__;
    try
    {
        SplusExecutionContext __context__ = SplusThreadStartCode(__SignalEventArg__);
        
        __context__.SourceCodeLine = 33;
        MYCONNECTION . Write ( TX .ToString()) ; 
        
        
    }
    catch(Exception e) { ObjectCatchHandler(e); }
    finally { ObjectFinallyHandler( __SignalEventArg__ ); }
    return this;
    
}

object ENABLEQUEUE_OnPush_3 ( Object __EventInfo__ )

    { 
    Crestron.Logos.SplusObjects.SignalEventArgs __SignalEventArg__ = (Crestron.Logos.SplusObjects.SignalEventArgs)__EventInfo__;
    try
    {
        SplusExecutionContext __context__ = SplusThreadStartCode(__SignalEventArg__);
        
        __context__.SourceCodeLine = 38;
        if ( Functions.TestForTrue  ( ( Functions.BoolToInt ( (Functions.TestForTrue ( Functions.BoolToInt (QUEUEMILLISECONDS == "") ) || Functions.TestForTrue ( Functions.BoolToInt (QUEUEMILLISECONDS == "0") )) ))  ) ) 
            { 
            __context__.SourceCodeLine = 40;
            QUEUEMILLISECONDS  .UpdateValue ( "1"  ) ; 
            } 
        
        __context__.SourceCodeLine = 42;
        MYCONNECTION . EnableQueue ( QUEUEMILLISECONDS .ToString()) ; 
        
        
    }
    catch(Exception e) { ObjectCatchHandler(e); }
    finally { ObjectFinallyHandler( __SignalEventArg__ ); }
    return this;
    
}

object ENABLEQUEUE_OnRelease_4 ( Object __EventInfo__ )

    { 
    Crestron.Logos.SplusObjects.SignalEventArgs __SignalEventArg__ = (Crestron.Logos.SplusObjects.SignalEventArgs)__EventInfo__;
    try
    {
        SplusExecutionContext __context__ = SplusThreadStartCode(__SignalEventArg__);
        
        __context__.SourceCodeLine = 47;
        MYCONNECTION . DisableQueue ( ) ; 
        
        
    }
    catch(Exception e) { ObjectCatchHandler(e); }
    finally { ObjectFinallyHandler( __SignalEventArg__ ); }
    return this;
    
}

public void RECEIVEDDATA ( object __sender__ /*TCPClient.TcpConnection SENDER */, TCPClient.SendArgs ARGS ) 
    { 
    TcpConnection  SENDER  = (TcpConnection )__sender__;
    try
    {
        SplusExecutionContext __context__ = SplusSimplSharpDelegateThreadStartCode();
        
        __context__.SourceCodeLine = 53;
        RX  .UpdateValue ( ARGS . Data  ) ; 
        
        
    }
    finally { ObjectFinallyHandler(); }
    }
    
public void CONNECTIONFEEDBACK ( object __sender__ /*TCPClient.TcpConnection SENDER */, TCPClient.SendArgs ARGS ) 
    { 
    TcpConnection  SENDER  = (TcpConnection )__sender__;
    try
    {
        SplusExecutionContext __context__ = SplusSimplSharpDelegateThreadStartCode();
        
        __context__.SourceCodeLine = 60;
        if ( Functions.TestForTrue  ( ( Functions.BoolToInt (ARGS.Data == "Connected"))  ) ) 
            { 
            __context__.SourceCodeLine = 62;
            CONNECTED  .Value = (ushort) ( 1 ) ; 
            } 
        
        else 
            {
            __context__.SourceCodeLine = 64;
            if ( Functions.TestForTrue  ( ( Functions.BoolToInt (ARGS.Data == "Could not Connect"))  ) ) 
                { 
                __context__.SourceCodeLine = 66;
                CONNECTED  .Value = (ushort) ( 0 ) ; 
                } 
            
            }
        
        
        
    }
    finally { ObjectFinallyHandler(); }
    }
    
public override object FunctionMain (  object __obj__ ) 
    { 
    try
    {
        SplusExecutionContext __context__ = SplusFunctionMainStartCode();
        
        __context__.SourceCodeLine = 74;
        // RegisterEvent( MYCONNECTION , DATARECEIVED , RECEIVEDDATA ) 
        try { g_criticalSection.Enter(); MYCONNECTION .DataReceived  += RECEIVEDDATA; } finally { g_criticalSection.Leave(); }
        ; 
        __context__.SourceCodeLine = 75;
        // RegisterEvent( MYCONNECTION , CONNECTIONSTATUS , CONNECTIONFEEDBACK ) 
        try { g_criticalSection.Enter(); MYCONNECTION .ConnectionStatus  += CONNECTIONFEEDBACK; } finally { g_criticalSection.Leave(); }
        ; 
        
        
    }
    catch(Exception e) { ObjectCatchHandler(e); }
    finally { ObjectFinallyHandler(); }
    return __obj__;
    }
    

public override void LogosSplusInitialize()
{
    SocketInfo __socketinfo__ = new SocketInfo( 1, this );
    InitialParametersClass.ResolveHostName = __socketinfo__.ResolveHostName;
    _SplusNVRAM = new SplusNVRAM( this );
    
    CONNECT = new Crestron.Logos.SplusObjects.DigitalInput( CONNECT__DigitalInput__, this );
    m_DigitalInputList.Add( CONNECT__DigitalInput__, CONNECT );
    
    ENABLEQUEUE = new Crestron.Logos.SplusObjects.DigitalInput( ENABLEQUEUE__DigitalInput__, this );
    m_DigitalInputList.Add( ENABLEQUEUE__DigitalInput__, ENABLEQUEUE );
    
    CONNECTED = new Crestron.Logos.SplusObjects.DigitalOutput( CONNECTED__DigitalOutput__, this );
    m_DigitalOutputList.Add( CONNECTED__DigitalOutput__, CONNECTED );
    
    PORTNUMBER = new Crestron.Logos.SplusObjects.AnalogInput( PORTNUMBER__AnalogSerialInput__, this );
    m_AnalogInputList.Add( PORTNUMBER__AnalogSerialInput__, PORTNUMBER );
    
    ADDRESS = new Crestron.Logos.SplusObjects.StringInput( ADDRESS__AnalogSerialInput__, 65534, this );
    m_StringInputList.Add( ADDRESS__AnalogSerialInput__, ADDRESS );
    
    TX = new Crestron.Logos.SplusObjects.StringInput( TX__AnalogSerialInput__, 65534, this );
    m_StringInputList.Add( TX__AnalogSerialInput__, TX );
    
    QUEUEMILLISECONDS = new Crestron.Logos.SplusObjects.StringInput( QUEUEMILLISECONDS__AnalogSerialInput__, 65534, this );
    m_StringInputList.Add( QUEUEMILLISECONDS__AnalogSerialInput__, QUEUEMILLISECONDS );
    
    RX = new Crestron.Logos.SplusObjects.StringOutput( RX__AnalogSerialOutput__, this );
    m_StringOutputList.Add( RX__AnalogSerialOutput__, RX );
    
    
    CONNECT.OnDigitalPush.Add( new InputChangeHandlerWrapper( CONNECT_OnPush_0, false ) );
    CONNECT.OnDigitalRelease.Add( new InputChangeHandlerWrapper( CONNECT_OnRelease_1, false ) );
    TX.OnSerialChange.Add( new InputChangeHandlerWrapper( TX_OnChange_2, false ) );
    ENABLEQUEUE.OnDigitalPush.Add( new InputChangeHandlerWrapper( ENABLEQUEUE_OnPush_3, false ) );
    ENABLEQUEUE.OnDigitalRelease.Add( new InputChangeHandlerWrapper( ENABLEQUEUE_OnRelease_4, false ) );
    
    _SplusNVRAM.PopulateCustomAttributeList( true );
    
    NVRAM = _SplusNVRAM;
    
}

public override void LogosSimplSharpInitialize()
{
    MYCONNECTION  = new TCPClient.TcpConnection();
    
    
}

public UserModuleClass_TCP_CLIENT_SHARP ( string InstanceName, string ReferenceID, Crestron.Logos.SplusObjects.CrestronStringEncoding nEncodingType ) : base( InstanceName, ReferenceID, nEncodingType ) {}




const uint ADDRESS__AnalogSerialInput__ = 0;
const uint TX__AnalogSerialInput__ = 1;
const uint QUEUEMILLISECONDS__AnalogSerialInput__ = 2;
const uint RX__AnalogSerialOutput__ = 0;
const uint PORTNUMBER__AnalogSerialInput__ = 3;
const uint CONNECT__DigitalInput__ = 0;
const uint ENABLEQUEUE__DigitalInput__ = 1;
const uint CONNECTED__DigitalOutput__ = 0;

[SplusStructAttribute(-1, true, false)]
public class SplusNVRAM : SplusStructureBase
{

    public SplusNVRAM( SplusObject __caller__ ) : base( __caller__ ) {}
    
    
}

SplusNVRAM _SplusNVRAM = null;

public class __CEvent__ : CEvent
{
    public __CEvent__() {}
    public void Close() { base.Close(); }
    public int Reset() { return base.Reset() ? 1 : 0; }
    public int Set() { return base.Set() ? 1 : 0; }
    public int Wait( int timeOutInMs ) { return base.Wait( timeOutInMs ) ? 1 : 0; }
}
public class __CMutex__ : CMutex
{
    public __CMutex__() {}
    public void Close() { base.Close(); }
    public void ReleaseMutex() { base.ReleaseMutex(); }
    public int WaitForMutex() { return base.WaitForMutex() ? 1 : 0; }
}
 public int IsNull( object obj ){ return (obj == null) ? 1 : 0; }
}


}
