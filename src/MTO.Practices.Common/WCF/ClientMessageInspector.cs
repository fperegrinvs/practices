namespace MTO.Practices.Common.WCF
{
    using System;
    using System.Collections.Generic;
    using System.ServiceModel;
    using System.ServiceModel.Channels;
    using System.ServiceModel.Description;
    using System.ServiceModel.Dispatcher;

    /// <summary>
    /// Inspetor de mensagens client-side genérico
    /// </summary>
    public class ClientMessageInspector : IClientMessageInspector, IEndpointBehavior
    {
        /// <summary>
        /// Último request dessa thread
        /// </summary>
        [ThreadStatic]
        private static string lastRequest;

        /// <summary>
        /// Último response dessa thread
        /// </summary>
        [ThreadStatic]
        private static string lastResponse;

        /// <summary>
        /// Indica se devemos capturar respostas, porque estas geralmente são grandes e ocuparão muita memória
        /// </summary>
        private bool captureResponse;

        /// <summary>
        /// Initializes a new instance of the <see cref="ClientMessageInspector"/> class.
        /// </summary>
        /// <param name="captureResponse">
        /// Se o inspector deve capturar responses
        /// </param>
        public ClientMessageInspector(bool captureResponse = false)
        {
            this.captureResponse = captureResponse;
        }

        /// <summary>
        /// Recupera e define o último request da thread corrente
        /// </summary>
        public static string LastRequest
        {
            get
            {
                return lastRequest;
            }

            set
            {
                lastRequest = value;
            }
        }

        /// <summary>
        /// Recupera e define o último response da thread corrente
        /// </summary>
        public static string LastResponse
        {
            get
            {
                return lastResponse;
            }

            set
            {
                lastResponse = value;
            }
        }

        /// <summary>
        /// Enables inspection or modification of a message before a request message is sent to a service.
        /// </summary>
        /// <returns>
        /// The object that is returned as the correlationState argument of the 
        /// <see cref="M:System.ServiceModel.Dispatcher.IClientMessageInspector.AfterReceiveReply(System.ServiceModel.Channels.Message@,System.Object)"/> 
        /// method. This is null if no correlation state is used.The best practice is to make this a <see cref="T:System.Guid"/> to ensure that no two 
        /// correlationState objects are the same.
        /// </returns>
        /// <param name="request">The message to be sent to the service.</param><param name="channel">The  client object channel.</param>
        public object BeforeSendRequest(ref Message request, IClientChannel channel)
        {
            if (request == null)
            {
                throw new ArgumentNullException("request");
            }

            LastRequest = request.ToString();
            LastResponse = string.Empty;

            // Não precisamos correlacionar estado, como as chamadas são síncronas e só há uma armazenada por vez, não há contra o que correlacionar
            return null;
        }

        /// <summary>
        /// Enables inspection or modification of a message after a reply message is received but prior to passing it back to the client application.
        /// </summary>
        /// <param name="reply">The message to be transformed into types and handed back to the client application.</param><param name="correlationState">Correlation state data.</param>
        public void AfterReceiveReply(ref Message reply, object correlationState)
        {
            if (reply == null)
            {
                throw new ArgumentNullException("reply");
            }

            if (this.captureResponse)
            {
                LastResponse = reply.ToString();
            }
        }

        #region Implementation of IEndpointBehavior

        /// <summary>
        /// Implement to confirm that the endpoint meets some intended criteria.
        /// </summary>
        /// <param name="endpoint">The endpoint to validate.</param>
        public void Validate(ServiceEndpoint endpoint)
        {
        }

        /// <summary>
        /// Implement to pass data at runtime to bindings to support custom behavior.
        /// </summary>
        /// <param name="endpoint">The endpoint to modify.</param><param name="bindingParameters">The objects that binding elements require to support the behavior.</param>
        public void AddBindingParameters(ServiceEndpoint endpoint, BindingParameterCollection bindingParameters)
        {
        }

        /// <summary>
        /// Implements a modification or extension of the service across an endpoint.
        /// </summary>
        /// <param name="endpoint">The endpoint that exposes the contract.</param><param name="endpointDispatcher">The endpoint dispatcher to be modified or extended.</param>
        public void ApplyDispatchBehavior(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher)
        {
        }

        /// <summary>
        /// Implements a modification or extension of the client across an endpoint.
        /// </summary>
        /// <param name="endpoint">The endpoint that is to be customized.</param><param name="clientRuntime">The client runtime to be customized.</param>
        public void ApplyClientBehavior(ServiceEndpoint endpoint, ClientRuntime clientRuntime)
        {
            if (clientRuntime == null)
            {
                throw new ArgumentNullException("clientRuntime");
            }

            clientRuntime.MessageInspectors.Add(this);
        }

        #endregion
    }
}
