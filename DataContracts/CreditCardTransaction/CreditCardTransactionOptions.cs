﻿using System;
using System.Runtime.Serialization;
using GatewayApiClient.DataContracts.EnumTypes;

namespace GatewayApiClient.DataContracts {

    [DataContract(Name = "CreditCardTransactionOptions", Namespace = "")]
    public class CreditCardTransactionOptions {

        /// <summary>
        /// Código do método de pagamento
        /// </summary>
        [DataMember]
        public int PaymentMethodCode { get; set; }

        #region CurrencyIso

        /// <summary>
        /// Moeda. Opções: BRL, EUR, USD, ARS, BOB, CLP, COP, UYU, MXN, PYG
        /// </summary>
        [DataMember(Name = "CurrencyIso")]
        private string CurrencyIsoField {
            get {
                if (this.CurrencyIso == null) { return null; }
                return this.CurrencyIso.Value.ToString();
            }
            set {
                if (value == null) {
                    this.CurrencyIso = null;
                }
                else {
                    this.CurrencyIso = (CurrencyIsoEnum)Enum.Parse(typeof(CurrencyIsoEnum), value);
                }
            }
        }

        /// <summary>
        /// Moeda. Opções: BRL, EUR, USD, ARS, BOB, CLP, COP, UYU, MXN, PYG
        /// </summary>
        public Nullable<CurrencyIsoEnum> CurrencyIso { get; set; }

        #endregion

        /// <summary>
        /// Taxa para a companhia aérea
        /// </summary>
        [DataMember]
        public long IataAmountInCents { get; set; }

        /// <summary>
        /// Indica o tempo que a transação deverá esperar até ser capturada (contando a partir da data de criação)
        /// </summary>
        [DataMember]
        public int CaptureDelayInMinutes { get; set; }

        /// <summary>
        /// Categoria da loja - MCC
        /// </summary>
        [DataMember]
        public Nullable<long> MerchantCategoryCode { get; set; }

        /// <summary>
        /// Nome que aparecerá na fatura do comprador (caso não informado, o texto configurado no gateway será utilizado)
        /// </summary>
        [DataMember]
        public string SoftDescriptorText { get; set; }

        /// <summary>
        /// Taxa de juros
        /// </summary>
        [DataMember]
        public Nullable<decimal> InterestRate { get; set; }

        /// <summary>
        /// Indica se o limite extendido está habilitado
        /// </summary>
        [DataMember]
        public Nullable<bool> ExtendedLimitEnabled { get; set; }

        /// <summary>
        /// Código do limite extendido
        /// </summary>
        [DataMember]
        public string ExtendedLimitCode { get; set; }
    }
}