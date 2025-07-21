///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace yourInvoice.Common.ErrorHandling
{
    [ExcludeFromCodeCoverage]
    public static class MessageHandler
    {
        public static class MessageCodes
        {
            public static string OfferNotExist
            {
                get { return "1000"; }
            }

            public static string FileRejectByCufe
            {
                get { return "1001"; }
            }

            public static string FileRejectByNoSameSeller
            {
                get { return "1002"; }
            }

            public static string FileRejectByNoSamePayer
            {
                get { return "1003"; }
            }

            public static string FileRejectByIsNotCreditType
            {
                get { return "1004"; }
            }

            public static string FileRejectByInvoiceExist
            {
                get { return "1005"; }
            }

            public static string FileRejectBySchemaDian
            {
                get { return "1006"; }
            }

            public static string FileRejectByNoHaveTwoFiles
            {
                get { return "1007"; }
            }

            public static string FileRejectByHaveVirus
            {
                get { return "1008"; }
            }

            public static string FileRejectByNoZip
            {
                get { return "1009"; }
            }

            public static string FileRejectByNoSendFiles
            {
                get { return "1010"; }
            }

            public static string FileRejectByIncorrectMoneyType
            {
                get { return "1011"; }
            }

            public static string PayerNotExist
            {
                get { return "1012"; }
            }

            public static string ParameterEmpty
            {
                get { return "1013"; }
            }

            public static string FileEmpty
            {
                get { return "1014"; }
            }

            public static string ErrorFile
            {
                get { return "1015"; }
            }

            public static string FileDianGenerated
            {
                get { return "1016"; }
            }

            public static string FileDianCreatedBlobStorage
            {
                get { return "1017"; }
            }

            public static string ServiceBusConexionNotExist
            {
                get { return "1018"; }
            }

            public static string ServiceBusConexionQueueCUFESNotExist
            {
                get { return "1019"; }
            }

            public static string FileRejectBySize
            {
                get { return "1020"; }
            }

            public static string FileRejectBySameCufe
            {
                get { return "1021"; }
            }

            public static string EndorsementInvoice
            {
                get { return "1022"; }
            }

            public static string AssociatedPaidEvent
            {
                get { return "1023"; }
            }

            public static string SeedFileFtp
            {
                get { return "1024"; }
            }

            public static string SearchFileFtp
            {
                get { return "1025"; }
            }

            public static string ProcessFileFtp
            {
                get { return "1026"; }
            }

            public static string ValidationColumnExcelDate
            {
                get { return "1027"; }
            }

            public static string ValidationColumnExcelNumeric
            {
                get { return "1028"; }
            }

            public static string FileRejectByNoContentSameCountRecordThatOffer
            {
                get { return "1029"; }
            }

            public static string FileRejectByContentInvoiceInvalids
            {
                get { return "1030"; }
            }

            public static string FileRejectByContentDateInvoiceInvalids
            {
                get { return "1031"; }
            }

            public static string FileRejectByContentValuesLessThatZero
            {
                get { return "1032"; }
            }

            public static string CatalogNotExist
            {
                get { return "1033"; }
            }

            public static string BeneficiaryExist
            {
                get { return "1034"; }
            }

            public static string BankCertificateRequired
            {
                get { return "1035"; }
            }

            public static string DocumentOrRutRequired
            {
                get { return "1036"; }
            }

            public static string NoPdf
            {
                get { return "1037"; }
            }

            public static string NitIsRequired
            {
                get { return "1038"; }
            }

            public static string DocumentIsSigned
            {
                get { return "1039"; }
            }

            public static string DocumentNotExist
            {
                get { return "1040"; }
            }

            public static string Beneficiaries10
            {
                get { return "1041"; }
            }

            public static string FieldEmpty
            {
                get { return "1042"; }
            }

            public static string EmailNotConfigured
            {
                get { return "1043"; }
            }

            public static string EmailNotValid
            {
                get { return "1044"; }
            }

            public static string EmailExpiredOffer
            {
                get { return "1045"; }
            }

            public static string FtpFactoringNoExistsInvoice
            {
                get { return "1046"; }
            }

            public static string FtpFactoringNoExistsPayer
            {
                get { return "1047"; }
            }

            public static string FtpFactoringNoExistsSeller
            {
                get { return "1048"; }
            }

            public static string FtpFactoringOfferProcessBuyer
            {
                get { return "1049"; }
            }

            public static string FtpFactoringFileProcess
            {
                get { return "1050"; }
            }

            public static string FileSize5
            {
                get { return "1051"; }
            }

            public static string FileSize10
            {
                get { return "1052"; }
            }

            public static string FtpFactoringNroInvoiceInvalid
            {
                get { return "1053"; }
            }

            public static string FtpFactoringHighestPurchaseDateFinalDate
            {
                get { return "1054"; }
            }

            public static string FtpFactoringPurchaseDateMinorWeek
            {
                get { return "1055"; }
            }

            public static string FtpFactoringFileNotExists
            {
                get { return "1056"; }
            }

            public static string FileRejectByEmptyField
            {
                get { return "1057"; }
            }

            public static string ZapsignError
            {
                get { return "1058"; }
            }

            public static string ZapsignNoToken
            {
                get { return "1059"; }
            }

            public static string NoExistResumen
            {
                get { return "1060"; }
            }

            public static string AccountExist
            {
                get { return "1061"; }
            }

            public static string AccountNotExist
            {
                get { return "1062"; }
            }

            public static string BuyerNotExist
            {
                get { return "1063"; }
            }

            public static string FactoringFieldEmpty
            {
                get { return "1064"; }
            }

            public static string FactoringReasignation
            {
                get { return "1065"; }
            }

            public static string TwoXmlInSameZip
            {
                get { return "1066"; }
            }

            public static string PaymentValuePercentagePlus
            {
                get { return "1067"; }
            }

            public static string MessegeEventDian030
            {
                get { return "1068"; }
            }

            public static string MessegeEventDian032
            {
                get { return "1069"; }
            }

            public static string MessegeEventDian033
            {
                get { return "1070"; }
            }

            public static string MessegeEventDian034
            {
                get { return "1071"; }
            }

            public static string MessegeEventDian031
            {
                get { return "1072"; }
            }

            public static string MessegeEventRadian036
            {
                get { return "1073"; }
            }

            public static string MessegeWhitEventRadian037
            {
                get { return "1074"; }
            }

            public static string MessegeWithoutEventRadian037
            {
                get { return "1075"; }
            }

            public static string MessegeRadian047
            {
                get { return "1076"; }
            }

            public static string MessegeRadian046
            {
                get { return "1077"; }
            }

            public static string MessegeRadian045
            {
                get { return "1078"; }
            }

            public static string MessageOfferIsNotInProgress
            {
                get { return "1079"; }
            }

            public static string GeneralInformationExist
            {
                get { return "1080"; }
            }

            public static string GeneralInformationNotExist
            {
                get { return "1081"; }
            }

            public static string MessageOfferIsPurchased
            {
                get { return "1082"; }
            }

            public static string MessageNoExistsCurrentUser
            {
                get { return "1083"; }
            }

            public static string MessageExistsInformation
            {
                get { return "1084"; }
            }

            public static string HigherAmount
            {
                get { return "1085"; }
            }

            public static string MessageNotIsLegal
            {
                get { return "1086"; }
            }

            public static string DocumentsAreSigned
            {
                get { return "1087"; }
            }

            public static string PendigApproval
            {
                get { return "1088"; }
            }

            public static string StatusLinkingNotCompleted
            {
                get { return "1089"; }
            }

            public static string ErrorInGenerateFile
            {
                get { return "1090"; }
            }

            public static string FileXmlInvalid
            {
                get { return "1091"; }
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="messageCode"></param>
        /// <returns></returns>
        public static string GetErrorDescription(string messageCode)
        {
            return MessageResource.ResourceManager.GetString(messageCode);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="messageCode"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static string GetErrorDescription(string messageCode, params object[] parameters)
        {
            return string.Format(CultureInfo.InvariantCulture, MessageResource.ResourceManager.GetString(messageCode), parameters);
        }
    }
}