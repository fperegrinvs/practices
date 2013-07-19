namespace MTO.Practices.Common.WCF
{
    using System;
    using System.ServiceModel;
    using System.ServiceModel.Channels;

    using MTO.Practices.Common.Exceptions;
    using MTO.Practices.Common.Interfaces;
    using MTO.Practices.Common.Wrappers;

    /// <summary>
    /// Classe base para wrapper de serviços
    /// </summary>
    /// <typeparam name="U">
    /// </typeparam>
    public abstract class ManualServiceBase<U>
        where U : class
    {
        /// <summary>
        /// Cliente do serviço
        /// </summary>
        protected ICommunicationObject Channel = null;

        public void InvokeService(Action<U> serviceMethod, WrapperOptions options = null, Action<Exception> serviceException = null)
        {
            try
            {
                var client = this.Prepare(options);
                serviceMethod(client);
                this.Channel.Close();
            }
            catch (EndpointNotFoundException ex)
            {
                this.Channel.Abort();
                throw new ServiceUnavailableException("Serviço indisponível no momento.", ex);
            }
            catch (TimeoutException ex)
            {
                this.Channel.Abort();
                throw new ServiceUnavailableException("Serviço indisponível no momento.", ex);
            }
            catch (ServerTooBusyException ex)
            {
                this.Channel.Abort();
                throw new ServiceUnavailableException("Serviço indisponível no momento.", ex);
            }
            catch (CommunicationException ex)
            {
                this.Channel.Abort();
                throw new ServiceUnavailableException("Serviço indisponível no momento.", ex);
            }
        }

        public T InvokeService<T>(Func<U, T> method, WrapperOptions options = null, Func<Exception, T> serviceException = null)
        {
            try
            {
                var client = this.Prepare(options);
                var ret = method(client);
                this.Channel.Close();
                return ret;
            }
            catch (EndpointNotFoundException ex)
            {
                this.Channel.Abort();
                throw new ServiceUnavailableException("Serviço indisponível no momento.", ex);
            }
            catch (TimeoutException ex)
            {
                this.Channel.Abort();
                throw new ServiceUnavailableException("Serviço indisponível no momento.", ex);
            }
            catch (ServerTooBusyException ex)
            {
                this.Channel.Abort();
                throw new ServiceUnavailableException("Serviço indisponível no momento.", ex);
            }
            catch (CommunicationException ex)
            {
                this.Channel.Abort();

                // O request é inválido.
                if (serviceException == null)
                {
                    this.ServerException(ex);
                    return default(T);
                }

                return serviceException(ex);
            }
        }

        /// <summary>
        /// Operação a ser executada quando servidor gerou uma exception
        /// </summary>
        public virtual void ServerException(Exception ex)
        {
            throw ex;
        }

        protected U Prepare(WrapperOptions options = null)
        {
            ICredential credentials = null;

            if (options != null && !string.IsNullOrWhiteSpace(options.CredentialName))
            {
                credentials = Context.Current.Credentials[options.CredentialName];
            }

            ChannelFactory<U> factory;
            if (options == null
                || string.IsNullOrWhiteSpace(options.ServiceName)
                || (credentials != null && string.IsNullOrWhiteSpace(credentials.Url)))
            {
                factory = new ChannelFactory<U>("*");
            }
            else if (!string.IsNullOrWhiteSpace(options.ServiceName))
            {
                factory = new ChannelFactory<U>(options.ServiceName);
            }
            else if (credentials != null)
            {
                Binding binding;
                if (!string.IsNullOrWhiteSpace(credentials.Login))
                {
                    binding = new BasicHttpBinding("BasicHttpBinding_IService");
                }
                else
                {
                    binding = new BasicHttpBinding(BasicHttpSecurityMode.None);
                }

                string url;

                url = (!string.IsNullOrWhiteSpace(credentials.Url) && !string.IsNullOrWhiteSpace(credentials.Login))
                          ? credentials.Url.Replace("http:", "https:")
                          : credentials.Url;

                factory = new ChannelFactory<U>(binding, url);
            }
            else
            {
                throw new Exception("Nenhum endereço definido para o serviço.");
            }

            if (options != null && credentials != null && !string.IsNullOrWhiteSpace(options.CredentialName) && Context.Current.Credentials.ContainsKey(options.CredentialName))
            {
                factory.Credentials.UserName.UserName = credentials.Login;
                factory.Credentials.UserName.Password = credentials.Password;
            }

            var channel = factory.CreateChannel();
            this.Channel = (ICommunicationObject)channel;
            return channel;
        }
    }
}
