﻿using System;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;

namespace GatewayApiClient.DataContracts {

    /// <summary>
    /// Gerenciar Venda
    /// </summary>
    [DataContract(Name = "ManageSaleRequest", Namespace = "")]
    public class ManageSaleRequest : BaseRequest {

        /// <summary>
        /// Coleções de transdações de cartão de crédito
        /// </summary>
        [DataMember]
        public Collection<ManageCreditCardTransaction> CreditCardTransactionCollection { get; set; }

        /// <summary>
        /// Chave do pedido. Utilizada para identificar um pedido no gateway
        /// </summary>
        [DataMember]
        public Nullable<Guid> OrderKey { get; set; }
    }
}