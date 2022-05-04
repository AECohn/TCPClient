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
using Simpl_Sharp_Pro_Template;
using Crestron.SimplSharp.Python;

namespace UserModule_TCP_CLIENT_SHARP
{
    public class UserModuleClass_TCP_CLIENT_SHARP : SplusObject
    {
        static CCriticalSection g_criticalSection = new CCriticalSection();
        
        Crestron.Logos.SplusObjects.StringInput ADDRESS;
        Crestron.Logos.SplusObjects.StringInput TX;
        Crestron.Logos.SplusObjects.StringInput RX;
        Crestron.Logos.SplusObjects.AnalogInput PORTNUMBER;
        Crestron.Logos.SplusObjects.DigitalInput CONNECT;
        Crestron.Logos.SplusObjects.DigitalInput DISCONNECT;
        Simpl_Sharp_Pro_Template.TcpConnection MYCONNECTION;
        
        public override void LogosSplusInitialize()
        {
            SocketInfo __socketinfo__ = new SocketInfo( 1, this );
            InitialParametersClass.ResolveHostName = __socketinfo__.ResolveHostName;
            _SplusNVRAM = new SplusNVRAM( this );
            
            CONNECT = new Crestron.Logos.SplusObjects.DigitalInput( CONNECT__DigitalInput__, this );
            m_DigitalInputList.Add( CONNECT__DigitalInput__, CONNECT );
            
            DISCONNECT = new Crestron.Logos.SplusObjects.DigitalInput( DISCONNECT__DigitalInput__, this );
            m_DigitalInputList.Add( DISCONNECT__DigitalInput__, DISCONNECT );
            
            PORTNUMBER = new Crestron.Logos.SplusObjects.AnalogInput( PORTNUMBER__AnalogSerialInput__, this );
            m_AnalogInputList.Add( PORTNUMBER__AnalogSerialInput__, PORTNUMBER );
            
            ADDRESS = new Crestron.Logos.SplusObjects.StringInput( ADDRESS__AnalogSerialInput__, 65534, this );
            m_StringInputList.Add( ADDRESS__AnalogSerialInput__, ADDRESS );
            
            TX = new Crestron.Logos.SplusObjects.StringInput( TX__AnalogSerialInput__, 65534, this );
            m_StringInputList.Add( TX__AnalogSerialInput__, TX );
            
            RX = new Crestron.Logos.SplusObjects.StringInput( RX__AnalogSerialInput__, 65534, this );
            m_StringInputList.Add( RX__AnalogSerialInput__, RX );
            
            
            
            _SplusNVRAM.PopulateCustomAttributeList( true );
            
            NVRAM = _SplusNVRAM;
            
        }
        
        public override void LogosSimplSharpInitialize()
        {
            MYCONNECTION  = new Simpl_Sharp_Pro_Template.TcpConnection();
            
            
        }
        
        public UserModuleClass_TCP_CLIENT_SHARP ( string InstanceName, string ReferenceID, Crestron.Logos.SplusObjects.CrestronStringEncoding nEncodingType ) : base( InstanceName, ReferenceID, nEncodingType ) {}
        
        
        
        
        const uint ADDRESS__AnalogSerialInput__ = 0;
        const uint TX__AnalogSerialInput__ = 1;
        const uint RX__AnalogSerialInput__ = 2;
        const uint PORTNUMBER__AnalogSerialInput__ = 3;
        const uint CONNECT__DigitalInput__ = 0;
        const uint DISCONNECT__DigitalInput__ = 1;
        
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
