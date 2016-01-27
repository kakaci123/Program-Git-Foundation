$(function () {
    $("#CityId").change(function () {
        $.ajax({
            type: "post",
            traditional: true,
            async: true,
            url: RootPath + "/shb/S26/Ajax_Area",
            data: { city: $(this).val(), zip: false, all: false },
            //dataType: "json",
            //jsonp: "callback",
            //jsonpCallback: "dool",
            success: function (data) {
                if (data) {
                    $("#AreaId").html(data);
                }
            }
        });
    });
});