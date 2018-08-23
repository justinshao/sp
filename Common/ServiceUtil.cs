using System;
using System.Net;
using System.Windows;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Collections.Generic;

namespace Server.Common
{
    public class ServiceUtil<T>
    {
        private static Uri GetNetTcpUri(string path)
        {
            Uri uri = null;
            if (Options.ServiceHost.Equals("localhost", StringComparison.InvariantCultureIgnoreCase))
            {
                uri = new Uri("net.tcp://localhost:" + Options.NetTcpPort.ToString() + "/" + path);
            }
            else
            {
                uri = new Uri("net.tcp://" + Options.ServiceHost + ":" + Options.NetTcpPort.ToString() + "/" + path);
            }
            return uri;
        }


        private static Uri GetHttpUri(string path)
        {
            Uri uri = null;
            if (Options.ServiceHost.Equals("localhost", StringComparison.InvariantCultureIgnoreCase))
            {
                uri = new Uri("http://localhost:" + Options.BasicHttpPort.ToString() + "/" + path);
            }
            else
            {
                uri = new Uri("http://" + Options.ServiceHost + ":" + Options.BasicHttpPort.ToString() + "/" + path);
            }
            return uri;
        }

        private static Uri GetNetTcpUri(string path, string ServerIP)
        {
            Uri uri = new Uri("net.tcp://" + ServerIP + ":" + Options.NetTcpPort.ToString() + "/" + path);
            return uri;
        }

        private static Uri GetHttpUri(string path, string ServerIP)
        {
            Uri uri = new Uri("http://" + ServerIP + ":" + Options.BasicHttpPort.ToString() + "/" + path);
            return uri;
        }

        /// <summary>
        /// 根据配置获取指定的服务
        /// </summary>
        /// <param name="servicePath">去除网址的服务地址</param>
        /// <param name="clientType">要创建的对象类型，可选参数，可忽略</param>
        /// <returns>对应的服务</returns>
        public static T GetServiceClient(string servicePath, Type clientType = null)
        {
            if (Options.Binding.Equals("Net.Tcp", StringComparison.InvariantCultureIgnoreCase))
            {
                return GetNetTcpClient(servicePath, clientType);
            }
            else
            {
                return GetBasicHttpClient(servicePath, clientType);
            }
        }

        /// <summary>
        /// 根据配置获取指定的服务
        /// </summary>
        /// <param name="servicePath">去除网址的服务地址</param>
        /// <param name="clientType">要创建的对象类型，可选参数，可忽略</param>
        /// <returns>对应的服务</returns>
        public static T GetServiceClient(string servicePath, string ServerIP, Type clientType = null)
        {
            if (Options.Binding.Equals("Net.Tcp", StringComparison.InvariantCultureIgnoreCase))
            {
                return GetNetTcpClient(servicePath, ServerIP, clientType);
            }
            else
            {
                return GetBasicHttpClient(servicePath, ServerIP, clientType);
            }
        }
        /// <summary>
        /// 获取Net.Tcp绑定的服务
        /// </summary>
        /// <param name="servicePath">去除网址的服务地址</param>
        /// <param name="clientType">要创建的对象类型，可选参数，可忽略</param>
        /// <returns>对应的服务</returns>
        public static T GetNetTcpClient(string servicePath, Type clientType = null)
        {
            T instance = default(T);
            object[] parameters = new object[2];

            var elements = new List<BindingElement>();
            elements.Add(new BinaryMessageEncodingBindingElement());
            var element = new TcpTransportBindingElement();
            element.MaxReceivedMessageSize = Int32.MaxValue;
            element.MaxBufferSize = Int32.MaxValue;

            elements.Add(element);
            CustomBinding customBinding = new CustomBinding(elements);
            customBinding.ReceiveTimeout = new TimeSpan(0, 10, 0);
            customBinding.SendTimeout = new TimeSpan(0, 10, 0);
            parameters[0] = customBinding;
            parameters[1] = new EndpointAddress(GetNetTcpUri(servicePath));

            System.Reflection.ConstructorInfo constructor = null;

            try
            {
                Type[] types = new Type[2];
                types[0] = typeof(System.ServiceModel.Channels.Binding);
                types[1] = typeof(System.ServiceModel.EndpointAddress);
                constructor = clientType == null ? typeof(T).GetConstructor(types) : clientType.GetConstructor(types);
            }
            catch (Exception)
            {
                return default(T);
            }

            if (constructor != null)
            {
                instance = (T)constructor.Invoke(parameters);
            }

            return instance;
        }

        /// <summary>
        /// 获取BasicHttp绑定的服务
        /// </summary>
        /// <param name="servicePath">去除网址的服务地址</param>
        /// <param name="clientType">要创建的对象类型，可选参数，可忽略</param>
        /// <returns>对应的服务</returns>
        public static T GetBasicHttpClient(string servicePath, Type clientType = null)
        {
            T instance = default(T);
            object[] parameters = new object[2];

            BasicHttpBinding binding = new BasicHttpBinding(BasicHttpSecurityMode.None);
            binding.MaxReceivedMessageSize = Int32.MaxValue;
            binding.MaxBufferSize = Int32.MaxValue;
            binding.ReceiveTimeout = new TimeSpan(0, 10, 0);
            binding.SendTimeout = new TimeSpan(0, 10, 0);
            binding.ReaderQuotas.MaxStringContentLength = Int32.MaxValue;
            parameters[0] = binding;
            parameters[1] = new EndpointAddress(GetHttpUri(servicePath));

            System.Reflection.ConstructorInfo constructor = null;

            try
            {
                Type[] types = new Type[2];
                types[0] = typeof(System.ServiceModel.Channels.Binding);
                types[1] = typeof(System.ServiceModel.EndpointAddress);
                constructor = clientType == null ? typeof(T).GetConstructor(types) : clientType.GetConstructor(types);
            }
            catch (Exception)
            {
                return default(T);
            }

            if (constructor != null)
            {
                instance = (T)constructor.Invoke(parameters);
            }

            return instance;
        }
        /// <summary>
        /// 获取Net.Tcp绑定的服务
        /// </summary>
        /// <param name="servicePath">去除网址的服务地址</param>
        /// <param name="clientType">要创建的对象类型，可选参数，可忽略</param>
        /// <returns>对应的服务</returns>
        public static T GetNetTcpClient(string servicePath, string ServerIP, Type clientType = null)
        {
            T instance = default(T);
            object[] parameters = new object[2];

            var elements = new List<BindingElement>();
            elements.Add(new BinaryMessageEncodingBindingElement());
            var element = new TcpTransportBindingElement();
            element.MaxReceivedMessageSize = Int32.MaxValue;
            element.MaxBufferSize = Int32.MaxValue;
            elements.Add(element);
            CustomBinding customBinding = new CustomBinding(elements);
            customBinding.ReceiveTimeout = new TimeSpan(0, 10, 0);
            customBinding.SendTimeout = new TimeSpan(0, 10, 0);
            parameters[0] = customBinding;
            parameters[1] = new EndpointAddress(GetNetTcpUri(servicePath, ServerIP));

            System.Reflection.ConstructorInfo constructor = null;

            try
            {
                Type[] types = new Type[2];
                types[0] = typeof(System.ServiceModel.Channels.Binding);
                types[1] = typeof(System.ServiceModel.EndpointAddress);
                constructor = clientType == null ? typeof(T).GetConstructor(types) : clientType.GetConstructor(types);
            }
            catch (Exception)
            {
                return default(T);
            }

            if (constructor != null)
            {
                instance = (T)constructor.Invoke(parameters);
            }

            return instance;
        }

        /// <summary>
        /// 获取BasicHttp绑定的服务
        /// </summary>
        /// <param name="servicePath">去除网址的服务地址</param>
        /// <param name="clientType">要创建的对象类型，可选参数，可忽略</param>
        /// <returns>对应的服务</returns>
        public static T GetBasicHttpClient(string servicePath, string ServerIP, Type clientType = null)
        {
            T instance = default(T);
            object[] parameters = new object[2];

            BasicHttpBinding binding = new BasicHttpBinding(BasicHttpSecurityMode.None);
            binding.MaxReceivedMessageSize = Int32.MaxValue;
            binding.MaxBufferSize = Int32.MaxValue;
            binding.ReceiveTimeout = new TimeSpan(0, 10, 0);
            binding.SendTimeout = new TimeSpan(0, 10, 0);
            binding.ReaderQuotas.MaxStringContentLength = Int32.MaxValue;
            parameters[0] = binding;
            parameters[1] = new EndpointAddress(GetHttpUri(servicePath, ServerIP));

            System.Reflection.ConstructorInfo constructor = null;

            try
            {
                Type[] types = new Type[2];
                types[0] = typeof(System.ServiceModel.Channels.Binding);
                types[1] = typeof(System.ServiceModel.EndpointAddress);
                constructor = clientType == null ? typeof(T).GetConstructor(types) : clientType.GetConstructor(types);
            }
            catch (Exception)
            {
                return default(T);
            }

            if (constructor != null)
            {
                instance = (T)constructor.Invoke(parameters);
            }

            return instance;
        }
    }
}
