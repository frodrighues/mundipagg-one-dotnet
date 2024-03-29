﻿using System;
using System.Runtime.Serialization;
using GatewayApiClient.DataContracts.EnumTypes;

namespace GatewayApiClient.DataContracts {

    [DataContract(Name = "CreditCard", Namespace = "")]
    public class CreditCard {

        /// <summary>
        /// Chave do cartão de crédito. Utilizado para identificar um cartão de crédito no gateway
        /// </summary>
        [DataMember]
        public Guid InstantBuyKey { get; set; }

        /// <summary>
        /// Número do cartão de crédito
        /// </summary>
        [DataMember]
        public string CreditCardNumber { get; set; }

        /// <summary>
        /// Titular do cartão
        /// </summary>
        [DataMember]
        public string HolderName { get; set; }

        /// <summary>
        /// Código de segurança - CVV
        /// </summary>
        [DataMember]
        public string SecurityCode { get; set; }

        /// <summary>
        /// Mês de expiração do cartão de crédito
        /// </summary>
        [DataMember]
        public int ExpMonth { get; set; }

        /// <summary>
        /// Ano de expiração do cartão de crédito
        /// </summary>
        [DataMember]
        public int ExpYear { get; set; }

        #region CreditCardBrand

        /// <summary>
        /// Bandeira do cartão de crédito
        /// </summary>
        [DataMember(Name = "CreditCardBrand")]
        private string CreditCardBrandField {
            get {
                if (this.CreditCardBrand == null) { return null; }
                return this.CreditCardBrand.Value.ToString();
            }
            set {
                if (value == null) {
                    this.CreditCardBrand = null;
                }
                else {
                    this.CreditCardBrand = (CreditCardBrandEnum)Enum.Parse(typeof(CreditCardBrandEnum), value);
                }
            }
        }

        /// <summary>
        /// Bandeira do cartão de crédito
        /// </summary>
        public Nullable<CreditCardBrandEnum> CreditCardBrand { get; set; }

        #endregion

        /// <summary>
        /// Endereço de cobrança
        /// </summary>
        [DataMember]
        public BillingAddress BillingAddress { get; set; }
    }
}