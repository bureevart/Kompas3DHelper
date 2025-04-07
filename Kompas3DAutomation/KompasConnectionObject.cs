using Kompas6API5;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Kompas3DAutomation
{
    public class KompasConnectionObject
    {
        private KompasObject _kompas;
        private bool _isConnected = false;

        public void Connect()
        {
            try
            {
                _kompas = (KompasObject)Marshal.GetActiveObject("KOMPAS.Application.5");
            }
            catch
            {
                Type t = Type.GetTypeFromProgID("KOMPAS.Application.5");
                _kompas = (KompasObject)Activator.CreateInstance(t);
            }

            _isConnected = true;
        }

        public void Connect(object kompas_)
        {
            _kompas = (KompasObject)kompas_;

            if (_kompas == null)
                throw new NullReferenceException();

            _isConnected = true;
        }

        public KompasObject Kompas => _kompas;

        public bool IsConnected => _isConnected;
    }
}
