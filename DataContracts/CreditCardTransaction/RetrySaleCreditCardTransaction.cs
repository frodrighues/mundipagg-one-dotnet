﻿using System;
using System.Runtime.Serialization;

namespace GatewayApiClient.DataContracts {

    /// <summary>
    /// Transação de cartão de crédito a ser retentada
    /// </summary>
    [DataContract(Name = "RetrySaleCreditCardTransaction", Namespace = "")]
    public class RetrySaleCreditCardTransaction {

        /// <summary>
        /// Chave da transação. Utilizada para identificar uma transação de cartão de crédito no gateway
        /// </summary>
        [DataMember]
        public Guid TransactionKey { get; set; }

        /// <summary>
        /// Código de segurança do cartão - CVV
        /// </summary>
        [DataMember]
        public string SecurityCode { get; set; }
    }
}