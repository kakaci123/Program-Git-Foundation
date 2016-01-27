$(function () {
    $("#all").click(function () {
        $(".delete").each(function () {
            if ($("#all").prop("checked"))
                $(this).prop("checked", $("#all").prop("checked"));
            else
                $(this).prop("checked", false);
        });
    });
    $("#CityId").change(function () {
        $.ajax({
            type: "post",
            traditional: true,
            async: true,
            url: "/shb/S26/Ajax_Area",
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