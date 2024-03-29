﻿using System.Runtime.Serialization;

namespace GatewayApiClient.DataContracts {

    /// <summary>
    /// Dados do pedido
    /// </summary>
    [DataContract(Name = "Order", Namespace = "")]
    public class Order {

        /// <summary>
        /// Identificador do pedido no sistema da loja
        /// </summary>
        [DataMember]
        public string OrderReference { get; set; }
    }
}

