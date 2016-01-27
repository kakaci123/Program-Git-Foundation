$(function () {
    $("forom").submit(function () {
        $("#Content").val(CKEDITOR.instances.Content.getData());
    });
    CKEDITOR.basePath = RootPath+"/Scripts/plugin/ckeditor/";
    CKEDITOR.config.contentsCss = RootPath + "/Scripts/plugin/ckeditor/contents.css";
    CKEDITOR.config.skin = 'kama,' + RootPath + '/Scripts/plugin/ckeditor/skins/kama/';

    $("#CategoryId").change(function () {
        $.ajax({
            type: "post",
            traditional: true,
            async: true,
            url: RootPath + "/shb/S26/Ajax_Area1",
            data: { categoryId: $(this).val(), all: false },
            beforesend: function ()
            {
                $("#FrontTitle").hide();
                $(this).addClass("load");
            },
            complete:function(){
                $("#FronTitle").show();
                $(this).removeClass("load");
            },
            //dataType: "json",
            //jsonp: "callback",
            //jsonpCallback: "dool",
            success: function (data) {
                if (data) {
                    $("#FrontTitle").html(data);
                }
            }
        });
    });

});