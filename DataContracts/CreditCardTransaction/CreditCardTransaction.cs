﻿using System;
using System.Runtime.Serialization;
using GatewayApiClient.DataContracts.EnumTypes;

namespace GatewayApiClient.DataContracts {

    /// <summary>
    /// Dados da transação de cartão de crédito
    /// </summary>
    [DataContract(Name = "CreditCardTransaction", Namespace = "")]
    public class CreditCardTransaction {

        /// <summary>
        /// Dados do cartão de crédito
        /// </summary>
        [DataMember]
        public CreditCard CreditCard { get; set; }

        /// <summary>
        /// Opções da transação.
        /// </summary>
        [DataMember]
        public CreditCardTransactionOptions Options { get; set; }

        /// <summary>
        /// Dados de recorrência
        /// </summary>
        [DataMember]
        public Recurrency Recurrency { get; set; }

        /// <summary>
        /// Valor original da transação em centavos
        /// </summary>
        [DataMember]
        public long AmountInCents { get; set; }

        /// <summary>
        /// Número de parcelas da transação.
        /// </summary>
        [DataMember]
        public int InstallmentCount { get; set; }

        #region CreditCardOperation

        /// <summary>
        /// Operação. Opções: Undefined, AuthOnly, AuthAndCapture, AuthAndCaptureWithDelay
        /// </summary>
        [DataMember(Name = "CreditCardOperation")]
        private string CreditCardOperationField {
            get {
                if (this.CreditCardOperation == null) { return null; }
                return this.CreditCardOperation.ToString();
            }
            set {
                if (value == null) {
                    this.CreditCardOperationField = null;
                }
                else {
                    this.CreditCardOperation = (CreditCardOperationEnum)Enum.Parse(typeof(CreditCardOperationEnum), value);
                }
            }
        }

        public Nullable<CreditCardOperationEnum> CreditCardOperation { get; set; }

        #endregion

        /// <summary>
        /// Referência da transãção no sistema da loja
        /// </summary>
        [DataMember]
        public string TransactionReference { get; set; }

        #region TransactionDateInMerchant

        /// <summary>
        /// Data de criação da transação no sistema da loja
        /// </summary>
        [DataMember(Name = "TransactionDateInMerchant")]
        private string TransactionDateInMerchantField {
            get {
                if (this.TransactionDateInMerchant == null) { return null; }
                return this.TransactionDateInMerchant.Value.ToString(ServiceConstants.DATE_TIME_FORMAT);
            }
            set {
                if (value == null) {
                    this.TransactionDateInMerchant = null;
                }
                else {
                    this.TransactionDateInMerchant = DateTime.ParseExact(value, ServiceConstants.DATE_TIME_FORMAT, null);
                }
            }
        }

        /// <summary>
        /// Data de criação da transação no sistema da loja
        /// </summary>
        public Nullable<DateTime> TransactionDateInMerchant { get; set; }

        #endregion
    }
}
