namespace yourInvoice.Link.UnitTest.LinkingProcess
{
    using yourInvoice.Common.Constant;
    using yourInvoice.Common.Entities;
    using System.Collections.Generic;

    public static class EmailToRequestDocumentData
    {
        public static CatalogItemInfo GetCatalogItemInfo => new CatalogItemInfo
        {
            Descripton = "<!DOCTYPE html>\r\n<html>\r\n  <head>\r\n    <title>Solicitud de documentos adicionales - yourInvoice Digital</title>\r\n    <meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\">\r\n    <link href='\r\nhttps://fonts.googleapis.com/css?family=Inter' rel='stylesheet'>\r\n    <style>\r\n      body {\r\n        font-family: 'Inter';\r\n        font-size: 14px;\r\n      }\r\n    </style>\r\n  </head>\r\n  <body bgcolor=\"#FFFFFF\" leftmargin=\"0\" topmargin=\"0\" marginwidth=\"0\" marginheight=\"0\" style=\"font-family: 'Inter', arial, sans-serif; font-size: 14px;\">\r\n    <!-- Save for Web Slices (Mail_finalizaciónProceso.png) -->\r\n    <table id=\"Table_01\" width=\"845\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" align=\"center\" style=\"background-color: #FFF; color: #212121; border-radius: 16px;\">\r\n      <tbody>\r\n        <tr style=\"border-radius: 16px 16px 0px 0px; background-color:#202357; display: flex; height: 131px; padding-left: 48px; align-items: center; gap: 10px;\">\r\n          <td valign=\"middle\">\r\n            <img style=\"vertical-align: middle\" src=\"\r\nhttps://CMSblobfiledllo.blob.core.windows.net/custompages/LogoyourInvoiceDigital.gif\" width=\"268\" alt=\"LogoyourInvoiceDigital\">\r\n          </td>\r\n        </tr>\r\n        <tr>\r\n          <td>\r\n            <table width=\"100%\" border=\"0\" style=\"text-align: center; padding: 0 16px\">\r\n              <tbody>\r\n                <tr>\r\n                  <td style=\"padding: 16px 48px 24px 48px\">\r\n                    <h3 style=\"font-size: 20px \">{{displayLabel}} {{displayName}}</h3>\r\n                    <p style=\"font-size: 14px \">{{displayMessage}}</p>\r\n                    <p style=\"font-size: 14px \">\r\n                      <strong>Equipo yourInvoice Digital</strong>\r\n                    </p>\r\n                  </td>\r\n                </tr>\r\n              </tbody>\r\n            </table>\r\n          </td>\r\n        </tr>\r\n        <tr style=\"border-radius: 0px 0px 16px 16px; background-color:#202357; display: flex; align-items: center;\">\r\n          <td style=\"color: #FFFFFF; font-size: 14px; margin: 0 auto; padding: 32px 0px\">\r\n            <p style=\"text-align: center;\">\r\n              <strong>yourInvoice</strong> @ {{year}}. Todos los derechos reservados.\r\n            </p>\r\n          </td>\r\n        </tr>\r\n      </tbody>\r\n    </table>\r\n    <!-- End Save for Web Slices -->\r\n  </body>\r\n</html>",
            Name = "Envio de Correo Documentos Adicionales",
        };

        public static IEnumerable<CatalogItemInfo> GetCatalogItemInfoList => new List<CatalogItemInfo>()
        {
            new CatalogItemInfo {
                CatalogName = string.Empty,
                Descripton = "587",
                Name = EmailCatalog.EmailPort
            },
            new CatalogItemInfo {
                CatalogName = string.Empty,
                Descripton = "smtp.gmail.com",
                Name = EmailCatalog.EmailServer
            },
            new CatalogItemInfo {
                CatalogName = string.Empty,
                Descripton = "yourInvoicenotification@gmail.com",
                Name = EmailCatalog.EmailUser
            },
            new CatalogItemInfo {
                CatalogName = string.Empty,
                Descripton = "ghkohgjmxfepfnbb",
                Name = EmailCatalog.EmailPwd
            },
            new CatalogItemInfo {
                CatalogName = string.Empty,
                Descripton = "yourInvoicenotification@gmail.com",
                Name = EmailCatalog.EmailFrom
            },
            new CatalogItemInfo {
                CatalogName = string.Empty,
                Descripton = "yourInvoicenotification@gmail.com",
                Name = EmailCatalog.EmailSender
            },
        };
    }
}
