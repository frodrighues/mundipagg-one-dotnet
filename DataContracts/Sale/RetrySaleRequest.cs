﻿using System;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;

namespace GatewayApiClient.DataContracts {

    /// <summary>
    /// Pedido de retentativa
    /// </summary>
    [DataContract(Name = "RetrySaleResquest", Namespace = "")]
    public class RetrySaleRequest : BaseRequest {

        /// <summary>
        /// Chave do pedido. Utilizada para identificar um pedido no gateway
        /// </summary>
        [DataMember]
        public Guid OrderKey { get; set; }

        /// <summary>
        /// Opções da transação.
        /// </summary>
        [DataMember]
        public RetrySaleOptions Options { get; set; }

        /// <summary>
        /// Lista de trasanções de cartão de crédito a serem retentadas
        /// </summary>
        [DataMember]
        public Collection<RetrySaleCreditCardTransaction> RetrySaleCreditCardTransactionCollection { get; set; }
    }
}