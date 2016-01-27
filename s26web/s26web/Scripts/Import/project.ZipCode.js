//================================================================================================
/// <reference path="_references.js" /> Ps.如果你要新增給IntelliSense用的js檔案, 請加在該檔案中
//================================================================================================

(function (app) {
    //===========================================================================================
    var current = app.ZipCode = {};
    //===========================================================================================

    jQuery.extend(app.ZipCode,
    {
        Initialize: function (actionUrls) {

            /// <summary>
            /// 初始化函式
            /// </summary>
            /// <param name="actionUrls"></param>

            jQuery.extend(project.ActionUrls, actionUrls);

            //上傳檔案事件處理
            current.UploadEventHandler();
        },

        UploadEventHandler: function () {
            /// <summary>
            /// 上傳匯入資料
            /// </summary>

            $("#UploadForm").ajaxForm({
                iframe: true,
                dataType: "json",
                success: function (result) {
                    $("#UploadForm").resetForm();
                    if (!result.Result) {
                        project.AlertErrorMessage("錯誤", result.Msg);
                        
                    }
                    else {
   
                        $('#ResultContent').html(result.Msg);

                        current.ImportData(result.Msg , result.SalesPromotionId);
                        //project.ShowMessageCallback("訊息", "檔案載入完成, 點選【確認】後開始進行資料匯入", function () {
                        //    current.ImportData(result.Msg);
                        //});
                    }
                },
                error: function (xhr, textStatus, errorThrown) {
                    $("#UploadForm").resetForm();
                    project.AlertErrorMessage("錯誤", "檔案上傳錯誤");
                }
            });
        },

        ImportData: function (savedFileName, SalesPromotionId) {
            //ImportData: function (savedFileName) {
            /// <summary>
            /// 資料匯入
            /// </summary>
            /// <param name="mainID"></param>

            $.ajax({
                type: 'post',
                url: '/shb/SalesCode/Import',
                data: { savedFileName: savedFileName, SalesPromotionId: SalesPromotionId },
                async: false,
                cache: false,
                dataType: 'json',
                success: function (data) {
                    if (data.Msg) {
                        project.AlertErrorMessage("錯誤", data.Msg);
                        $('#UploadModal').modal('hide');
                    }
                    else {
                        project.ShowMessageCallback("訊息", "成功匯入 " + data.RowCount+" 筆序號", function () {
                            $('#UploadModal').modal('hide');
                            window.location.reload();
                        });
                    }
                },
                error: function () {
                    project.AlertErrorMessage("錯誤", "資料匯入發生錯誤");
                    $('#UploadModal').modal('hide');
                }
            });
        },
    });
})
(project);