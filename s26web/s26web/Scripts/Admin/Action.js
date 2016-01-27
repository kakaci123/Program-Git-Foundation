$(function () {
    $("#edit_action").change(function () {
        $("#action").attr("action", $(this).val());
    });

    $("#city").change(function () {
        $.ajax({
            type: "post",
            traditional: true,
            async: true,
            url: RootPath + "/shb/Illuma/Ajax_Area",
            data: { city: $(this).val(),zip:true,all:false },
            //dataType: "json",
            //jsonp: "callback",
            //jsonpCallback: "dool",
            success: function (data) {
                if (data) {
                    $("#area").html(data);
                }
            }
        });
    });

});