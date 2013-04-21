var progress = 0;
var percentage = 0;

function FetchDetails() {
    percentage = Math.ceil(100 / $('input[name$="hfId"]').length);
    progress = 0;
    $('input[name$="hfId"]').each(function () {
        var id = $(this).val();
        var field = $(this).parent().next().next().next().next().next();
        if (field.html() == "0" && field.next().html() == "0,00") {
            $.ajax({
                type: "POST",
                url: "Default.aspx/FetchDetails",
                data: '{"id":"' + id + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    if (msg.d != null) {
                        progress += percentage;
                        $('input[name$="hfId"][value="' + msg.d.id + '"]').parent().next().next().next().next().next().html(msg.d.bitrate).next().html(msg.d.size);
                        $("#progressbar").progressbar("option", "value", progress);
                    }
                }
            });
        }
        else {
            progress += percentage;
            $("#progressbar").progressbar("option", "value", progress);
        }
    });
}

$(function () {
    // Bind buttons
    $("#btnSearch").mousedown(function () {
        $(this).attr("src", "Images/BigSearch/BigSearchRightDown.png");
    }).mouseout(function () {
        $(this).attr("src", "Images/BigSearch/BigSearchRight.png");
    });
    $("#btnSearch2").mousedown(function () {
        $(this).attr("src", "Images/SmallSearch/SmallSearchRightDown.png");
    }).mouseout(function () {
        $(this).attr("src", "Images/SmallSearch/SmallSearchRight.png");
    });
    $(".download").mousedown(function () {
        $(this).attr("src", "Images/Button/DownloadDown.png");
    }).mouseout(function () {
        $(this).attr("src", "Images/Button/Download.png");
    });

    // Focus on search field
    $("#txtSearch2").focus();
    $("#txtSearch").focus();

    // Initialize progress bar
    $("#progressbar").progressbar({
        value: 0
    });

    // Fetch details
    setTimeout("FetchDetails()", 50);

    // Show sort arrow
    $('div[class^=a_]').removeClass("asc").removeClass("none").removeClass("desc");
    $(".a_" + sortexp).addClass(sortasc ? "asc" : "desc");
});