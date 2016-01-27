using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;

namespace s26web
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/js/AdminIndex").Include("~/Scripts/RootPath.js",
                                                               "~/Scripts/jQuery/jquery-1.10.1.js",
                                                               "~/Scripts/plugin/superfish.js",
                                                               "~/Scripts/plugin/date.js",
                                                               "~/Scripts/time.js"));
            bundles.Add(new StyleBundle("~/css/AdminIndex").Include("~/Content/Admin/Site.css",
                                                               "~/Content/Admin/superfish.css"));

            //=======================
            bundles.Add(new ScriptBundle("~/js/AdminContent").Include("~/Scripts/Admin/Content.js",
                                                                   "~/Scripts/jQuery/jquery-ui-1.10.3.js",
                                                                   "~/Scripts/plugin/TimePicker/jquery-ui-timepicker-addon.js",
                                                                   "~/Scripts/plugin/TimePicker/jquery.ui.datepicker-zh-TW.js",
                                                                   "~/Scripts/plugin/TimePicker/jquery-ui-timepicker-zh-TW.js"));
            bundles.Add(new StyleBundle("~/css/AdminContent").Include("~/Content/shb.css",
                                                                      "~/Content/Admin/Content.css",
                                                                      "~/Content/Admin/Web/MemberCategory.css",
                                                                      "~/Scripts/plugin/TimePicker/jquery-ui-1.8.16.custom.css",
                                                                      "~/Scripts/plugin/TimePicker/jquery-ui-timepicker-addon.css"
                                                                      ));

            //==================================================================================================
            bundles.Add(new ScriptBundle("~/js/AdminMemberIndex").Include("~/Scripts/Admin/Content.js",
                                                                        "~/Scripts/Admin/Action.js"));

            bundles.Add(new StyleBundle("~/css/AdminMemberIndex").Include("~/Content/Admin/Content.css",
                                                                    "~/Content/Admin/Web/MemberIndex.css"));
            //=======================
            bundles.Add(new ScriptBundle("~/js/AdminLoginIndex").Include("~/Scripts/Admin/Content.js",
                                                                       "~/Scripts/Admin/Web/NewsIndex.js",
                                                                       "~/Scripts/jQuery/jquery-ui-1.10.3.js",
                                                                       "~/Scripts/plugin/TimePicker/jquery-ui-timepicker-addon.js",
                                                                       "~/Scripts/plugin/TimePicker/jquery.ui.datepicker-zh-TW.js",
                                                                       "~/Scripts/plugin/TimePicker/jquery-ui-timepicker-zh-TW.js"));

            bundles.Add(new StyleBundle("~/css/AdminLoginIndex").Include("~/Content/Admin/Content.css",
                                                                    "~/Content/Admin/Web/LoginIndex.css",
                                                                    "~/Scripts/plugin/TimePicker/jquery-ui-1.8.16.custom.css",
                                                                    "~/Scripts/plugin/TimePicker/jquery-ui-timepicker-addon.css"));
            //=======================
            bundles.Add(new StyleBundle("~/css/AdminLogin").Include("~/Content/Admin/Content.css",
                                                        "~/Content/shb.css",
                                                        "~/Content/Admin/Web/Login.css"));
            //=======================
            bundles.Add(new StyleBundle("~/css/AdminMemberCategory").Include("~/Content/Admin/Content.css",
                                                              "~/Content/Admin/Web/MemberCategory.css"));
            //=======================
            bundles.Add(new StyleBundle("~/css/AdminECRM").Include("~/Content/Admin/Content.css",
                                                                   "~/Content/Admin/Web/eCRM.css"));
            //=======================
            bundles.Add(new StyleBundle("~/css/Index").Include("~/Content/Web/style.css",
                "~/Content/Web/css.css", "~/Content/Web/common.css", "~/Content/Web/main.css",
                                                                   "~/Content/Web/example.css"));
            //==================================================================================================
            bundles.Add(new ScriptBundle("~/js/Validate").Include("~/Scripts/jQuery/jquery.validate.js",
                                                                                 "~/Scripts/jQuery/jquery.validate.unobtrusive.js"));

            //==================================================================================================
            bundles.Add(new ScriptBundle("~/js/AdminVolunteersIndex").Include("~/Scripts/Admin/Content.js",
                                                                        "~/Scripts/Admin/Action.js",
                                                                        "~/Scripts/plugin/kabbar.jquery.js",
                                                                        "~/Scripts/Admin/Web/VolunteersIndex.js",
                                                                        "~/Scripts/jQuery/jquery-ui-1.10.3.js",
                                                                        "~/Scripts/plugin/TimePicker/jquery-ui-timepicker-addon.js",
                                                                        "~/Scripts/plugin/TimePicker/jquery.ui.datepicker-zh-TW.js",
                                                                        "~/Scripts/plugin/TimePicker/jquery-ui-timepicker-zh-TW.js",
                                                                        "~/Scripts/jQuery/jquery.transit.min.js"));
            bundles.Add(new StyleBundle("~/css/AdminVolunteersIndex").Include("~/Content/Admin/Content.css",
                                                                    "~/Content/Admin/Web/VolunteersIndex.css",
                                                                    "~/Scripts/plugin/TimePicker/jquery-ui-1.8.16.custom.css",
                                                                    "~/Scripts/plugin/TimePicker/jquery-ui-timepicker-addon.css"));
            //==================================================================================================
            bundles.Add(new StyleBundle("~/css/Page").Include("~/Content/Admin/Page.css"));
            bundles.Add(new StyleBundle("~/css/JQueryui").Include(
                        "~/Content/themes/base/jquery.ui.core.css",
                        "~/Content/themes/base/jquery.ui.resizable.css",
                        "~/Content/themes/base/jquery.ui.selectable.css",
                        "~/Content/themes/base/jquery.ui.accordion.css",
                        "~/Content/themes/base/jquery.ui.autocomplete.css",
                        "~/Content/themes/base/jquery.ui.button.css",
                        "~/Content/themes/base/jquery.ui.dialog.css",
                        "~/Content/themes/base/jquery.ui.slider.css",
                        "~/Content/themes/base/jquery.ui.tabs.css",
                        "~/Content/themes/base/jquery.ui.datepicker.css",
                        "~/Content/themes/base/jquery.ui.progressbar.css",
                        "~/Content/themes/base/jquery.ui.theme.css"));

            //==================================================================================================
            bundles.Add(new ScriptBundle("~/js/AddressAjax").Include("~/Scripts/Address.js"));

            //==================================================================================================
            bundles.Add(new ScriptBundle("~/js/AdminVolunteerEdit").Include("~/Scripts/Admin/Content.js",
                                                                            "~/Scripts/jQuery/jquery-ui-1.10.3.js",
                                                                            "~/Scripts/Admin/Web/VolunteerEdit.js",
                                                                           "~/Scripts/jQuery/jquery.unobtrusive*",
                                                                           "~/Scripts/jQuery/jquery.validate*",
                                                                           "~/Scripts/plugin/kabbar.jquery.js"));

            bundles.Add(new StyleBundle("~/css/AdminVolunteerEdit").Include("~/Content/Admin/Content.css",
                                                                            "~/Content/themes/base/jquery.ui.all.css"));
            //==================================================================================================
            bundles.Add(new ScriptBundle("~/js/AdminMemberCreate").Include("~/Scripts/Admin/Web/MemberCreate.js",
                                                                           "~/Scripts/plugin/DateSelect.js",
                                                                           "~/Scripts/jQuery/jquery.unobtrusive*",
                                                                           "~/Scripts/jQuery/jquery.validate*",
                                                                           "~/Scripts/plugin/kabbar.jquery.js"));
            //==================================================================================================
            bundles.Add(new ScriptBundle("~/js/Admin_Introduction").Include("~/Scripts/plugin/ckeditor/ckeditor.js",
                "~/Scripts/plugin/ckeditor/start.js",
                                                                        "~/Scripts/Admin/Web/Introduction.js",
                                                                        "~/Scripts/jQuery/jquery.unobtrusive*",
                                                                        "~/Scripts/jQuery/jquery.validate*"));
            //==================================================================================================
            bundles.Add(new ScriptBundle("~/js/AdminMemberIndex").Include("~/Scripts/Admin/Content.js",
                                                                  "~/Scripts/Admin/Action.js"));

            bundles.Add(new StyleBundle("~/css/AdminMemberIndex").Include("~/Content/Admin/Content.css",
                                                                    "~/Content/Admin/Web/MemberIndex.css"));
            //==================================================================================================
            bundles.Add(new ScriptBundle("~/js/AdminLoginIndex").Include("~/Scripts/Admin/Content.js",
                                                                       "~/Scripts/Admin/Web/NewsIndex.js",
                                                                       "~/Scripts/jQuery/jquery-ui-1.10.3.js",
                                                                       "~/Scripts/plugin/TimePicker/jquery-ui-timepicker-addon.js",
                                                                       "~/Scripts/plugin/TimePicker/jquery.ui.datepicker-zh-TW.js",
                                                                       "~/Scripts/plugin/TimePicker/jquery-ui-timepicker-zh-TW.js"));

            bundles.Add(new StyleBundle("~/css/AdminLoginIndex").Include("~/Content/Admin/Content.css",
                                                                    "~/Content/Admin/Web/LoginIndex.css",
                                                                    "~/Scripts/plugin/TimePicker/jquery-ui-1.8.16.custom.css",
                                                                    "~/Scripts/plugin/TimePicker/jquery-ui-timepicker-addon.css"));
            //==================================================================================================
            bundles.Add(new StyleBundle("~/css/AdminMemberCategory").Include("~/Content/Admin/Content.css",
                                                              "~/Content/Admin/Web/MemberCategory.css"));

            //==================================================================================================
            bundles.Add(new StyleBundle("~/css/AdminECRM").Include("~/Content/Admin/Content.css",
                                                                   "~/Content/Admin/Web/eCRM.css"));
            //==================================================================================================
            bundles.Add(new ScriptBundle("~/js/AdminQuestionnaireIndex").Include("~/Scripts/Admin/Content.js",
                                                                            "~/Scripts/Admin/Web/QuestionnaireIndex.js",
                                                                            "~/Scripts/jQuery/jquery-ui-1.10.3.js",
                                                                            "~/Scripts/plugin/TimePicker/jquery-ui-timepicker-addon.js",
                                                                            "~/Scripts/plugin/TimePicker/jquery.ui.datepicker-zh-TW.js",
                                                                            "~/Scripts/plugin/TimePicker/jquery-ui-timepicker-zh-TW.js"));

            bundles.Add(new StyleBundle("~/css/AdminQuestionnaireIndex").Include("~/Content/Admin/Content.css",
                                                                    "~/Content/Admin/Web/QuestionnaireIndex.css",
                                                                    "~/Scripts/plugin/TimePicker/jquery-ui-1.8.16.custom.css",
                                                                    "~/Scripts/plugin/TimePicker/jquery-ui-timepicker-addon.css"));
            //==================================================================================================
            bundles.Add(new ScriptBundle("~/js/AdminQuestionnaireView").Include("~/Scripts/Admin/Content.js",
                                                                  "~/Scripts/Admin/Action.js"));

            bundles.Add(new StyleBundle("~/css/AdminQuestionnaireView").Include("~/Content/Admin/Content.css",
                                                                    "~/Content/Admin/Web/QuestionnaireView.css"));
            //==================================================================================================

            bundles.Add(new ScriptBundle("~/js/AdminProductsIndex").Include("~/Scripts/Admin/Content.js",
                                                                        "~/Scripts/Admin/Action.js",
                                                                        "~/Scripts/Admin/Web/ProductsIndex.js",
                                                                        "~/Scripts/jQuery/jquery-ui-1.10.3.js",
                                                                        "~/Scripts/plugin/TimePicker/jquery-ui-timepicker-addon.js",
                                                                        "~/Scripts/plugin/TimePicker/jquery.ui.datepicker-zh-TW.js",
                                                                        "~/Scripts/plugin/TimePicker/jquery-ui-timepicker-zh-TW.js"));

            bundles.Add(new StyleBundle("~/css/AdminProductsIndex").Include("~/Content/Admin/Content.css",
                                                                    "~/Content/Admin/Web/ProductIndex.css",
                                                                    "~/Scripts/plugin/TimePicker/jquery-ui-1.8.16.custom.css",
                                                                    "~/Scripts/plugin/TimePicker/jquery-ui-timepicker-addon.css"));
            //==================================================================================================
            bundles.Add(new ScriptBundle("~/js/AdminPointIndex").Include("~/Scripts/Admin/Content.js",
                                                                        "~/Scripts/Admin/Action.js",
                                                                        "~/Scripts/Admin/Web/PointIndex.js",
                                                                        "~/Scripts/jQuery/jquery-ui-1.10.3.js",
                                                                        "~/Scripts/plugin/TimePicker/jquery-ui-timepicker-addon.js",
                                                                        "~/Scripts/plugin/TimePicker/jquery.ui.datepicker-zh-TW.js",
                                                                        "~/Scripts/plugin/TimePicker/jquery-ui-timepicker-zh-TW.js"));

            bundles.Add(new StyleBundle("~/css/AdminPointIndex").Include("~/Content/Admin/Content.css",
                                                                    "~/Content/Admin/Web/PointIndex.css",
                                                                    "~/Scripts/plugin/TimePicker/jquery-ui-1.8.16.custom.css",
                                                                    "~/Scripts/plugin/TimePicker/jquery-ui-timepicker-addon.css"));
            //==================================================================================================
            bundles.Add(new ScriptBundle("~/js/MemberForgotPassword").Include("~/Scripts/Web/MemberForgotPassword.js"));

            //==================================================================================================

            bundles.Add(new StyleBundle("~/js/AdminOrdersIndex").Include("~/Scripts/Admin/Content.js",
                                                                    "~/Scripts/Web/StatesOptionSelect.js",
                                                                    "~/Scripts/jQuery/jquery-ui-1.10.3.js",
                                                                            "~/Scripts/Admin/Web/OrdersIndex.js",
                                                                            "~/Scripts/plugin/TimePicker/jquery-ui-timepicker-addon.js",
                                                                            "~/Scripts/plugin/TimePicker/jquery.ui.datepicker-zh-TW.js",
                                                                            "~/Scripts/plugin/TimePicker/jquery-ui-timepicker-zh-TW.js"));


            bundles.Add(new StyleBundle("~/css/AdminOrdersIndex").Include("~/Content/Admin/Content.css",
                                                                          "~/Scripts/plugin/TimePicker/jquery-ui-1.8.16.custom.css",
                                                                          "~/Scripts/plugin/TimePicker/jquery-ui-timepicker-addon.css",
                                                                         "~/Content/Admin/Web/OrdersIndex.css"));
            //==================================================================================================

            bundles.Add(new StyleBundle("~/js/AdminExchangeGiftIndex").Include("~/Scripts/Admin/Content.js",
                                                                    "~/Scripts/Web/StatesOptionSelect.js",
                                                                    "~/Scripts/jQuery/jquery-ui-1.10.3.js",
                                                                            "~/Scripts/Admin/Web/ExchangeGiftIndex.js",
                                                                            "~/Scripts/plugin/TimePicker/jquery-ui-timepicker-addon.js",
                                                                            "~/Scripts/plugin/TimePicker/jquery.ui.datepicker-zh-TW.js",
                                                                            "~/Scripts/plugin/TimePicker/jquery-ui-timepicker-zh-TW.js"));


             bundles.Add(new StyleBundle("~/css/AdminExchangeGiftIndex").Include("~/Content/Admin/Content.css",
                                                                          "~/Scripts/plugin/TimePicker/jquery-ui-1.8.16.custom.css",
                                                                          "~/Scripts/plugin/TimePicker/jquery-ui-timepicker-addon.css",
                                                                         "~/Content/Admin/Web/ExchangeGiftIndex.css"));
            //==================================================================================================
            
            bundles.Add(new StyleBundle("~/css/AdminOrdersView").Include("~/Content/Admin/Content.css",
                                                        "~/Content/Admin/Web/OrdersView.css"));


            bundles.Add(new StyleBundle("~/js/AdminOrdersView").Include("~/Scripts/Web/StatesOptionSelect.js"));
            //==================================================================================================

            bundles.Add(new ScriptBundle("~/js/AdminGiftIndex").Include("~/Scripts/Admin/Content.js",
                                                                        "~/Scripts/Admin/Action.js",
                                                                        "~/Scripts/Admin/Web/GiftIndex.js",
                                                                        "~/Scripts/jQuery/jquery-ui-1.10.3.js",
                                                                        "~/Scripts/plugin/TimePicker/jquery-ui-timepicker-addon.js",
                                                                        "~/Scripts/plugin/TimePicker/jquery.ui.datepicker-zh-TW.js",
                                                                        "~/Scripts/plugin/TimePicker/jquery-ui-timepicker-zh-TW.js"));

            bundles.Add(new StyleBundle("~/css/AdminGiftIndex").Include("~/Content/Admin/Content.css",
                                                                    "~/Content/Admin/Web/GiftIndex.css",
                                                                    "~/Scripts/plugin/TimePicker/jquery-ui-1.8.16.custom.css",
                                                                    "~/Scripts/plugin/TimePicker/jquery-ui-timepicker-addon.css"));
            //==================================================================================================
            bundles.Add(new ScriptBundle("~/js/AdminSalesPromotionIndex").Include("~/Scripts/Admin/Content.js",
                                                                            "~/Scripts/Admin/Web/SalesPromotionIndex.js",
                                                                            "~/Scripts/jQuery/jquery-ui-1.10.3.js",
                                                                            "~/Scripts/plugin/TimePicker/jquery-ui-timepicker-addon.js",
                                                                            "~/Scripts/plugin/TimePicker/jquery.ui.datepicker-zh-TW.js",
                                                                            "~/Scripts/plugin/TimePicker/jquery-ui-timepicker-zh-TW.js"));

            bundles.Add(new StyleBundle("~/css/AdminSalesPromotionIndex").Include("~/Content/Admin/Content.css",
                                                                    "~/Content/Admin/Web/SalesPromotionIndex.css",
                                                                    "~/Scripts/plugin/TimePicker/jquery-ui-1.8.16.custom.css",
                                                                    "~/Scripts/plugin/TimePicker/jquery-ui-timepicker-addon.css"));
            //==================================================================================================
            bundles.Add(new ScriptBundle("~/js/AdminSalesCodeIndex").Include("~/Scripts/Admin/Content.js",
                "~/Scripts/Admin/Web/SalesCodeIndex.js",
                "~/Scripts/jQuery/jquery-ui-1.10.3.js",
                "~/Scripts/plugin/TimePicker/jquery-ui-timepicker-addon.js",
                "~/Scripts/plugin/TimePicker/jquery.ui.datepicker-zh-TW.js",
                "~/Scripts/plugin/TimePicker/jquery-ui-timepicker-zh-TW.js"));

            bundles.Add(new StyleBundle("~/css/AdminSalesCodeIndex").Include("~/Content/Admin/Content.css",
                   "~/Scripts/jquery-{version}.js",
                    "~/Content/Import/bootstrap.css",
                     "~/Content/Import/jasny-bootstrap.css",
                     "~/Content/Admin/Web/SalesCodeIndex.css",
                     "~/Scripts/plugin/TimePicker/jquery-ui-1.8.16.custom.css",
                     "~/Scripts/plugin/TimePicker/jquery-ui-timepicker-addon.css"));

            bundles.Add(new ScriptBundle("~/bundles/Import").Include(
                      "~/Scripts/Import/bootstrap.js",
                      "~/Scripts/Import/jasny-bootstrap.js",
                      "~/Scripts/Import/respond.js"));
            //==================================================================================================
            bundles.Add(new ScriptBundle("~/js/AdminOnlineIndex").Include("~/Scripts/Admin/Content.js",
                "~/Scripts/Admin/Web/OnlineIndex.js",
                "~/Scripts/jQuery/jquery-ui-1.10.3.js",
                "~/Scripts/plugin/TimePicker/jquery-ui-timepicker-addon.js",
                "~/Scripts/plugin/TimePicker/jquery.ui.datepicker-zh-TW.js",
                "~/Scripts/plugin/TimePicker/jquery-ui-timepicker-zh-TW.js"));

            bundles.Add(new StyleBundle("~/css/AdminOnlineIndex").Include("~/Content/Admin/Content.css",
                "~/Content/Admin/Web/OnlineIndex.css",
                "~/Scripts/plugin/TimePicker/jquery-ui-1.8.16.custom.css",
                "~/Scripts/plugin/TimePicker/jquery-ui-timepicker-addon.css"));
            //==================================================================================================
            bundles.Add(new ScriptBundle("~/js/DeliveryRecipient").Include("~/Scripts/Admin/Content.js",
                                                                            "~/Scripts/jQuery/jquery-ui-1.10.3.js",
                                                                            "~/Scripts/Front/Recipient.js",
                                                                           "~/Scripts/jQuery/jquery.unobtrusive*",
                                                                           "~/Scripts/jQuery/jquery.validate*",
                                                                           "~/Scripts/plugin/kabbar.jquery.js"));
        }
    }
}