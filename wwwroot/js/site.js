$(document).ready(function () {
    $('#phone').mask("8(999)999-99-99", { placeholder: "8(___)___-__-__" });
});

$(document).ready(function () {
    $("#SearchValue").on("keyup", function () {
        var SearchBy = $("#SearchBy").val();
        var SearchValue = $("#SearchValue").val();
        var CurrentPageIndex = $("#hfCurrentPageIndex").val() ? $("#hfCurrentPageIndex").val() : "1";
        $.ajax({
            type: "get",
            url: "/?currentPageIndex=" + CurrentPageIndex + "&SearchBy=" + SearchBy + "&SearchValue=" + SearchValue,
            contentType: "html",
            success: function (response) {
                $('#Data').html(jQuery(response).find('#Data').html());
                $('#Pagination').html(jQuery(response).find('#Pagination').html());
                history.pushState('', '', this.url);

            }
        })
    }
    )
})


$(document).ready(function () {
    $(document).on("click", ".page-link", function (e) {
        var SearchBy = $("#SearchBy").val();
        var SearchValue = $("#SearchValue").val();
        var NextPageIndex = e.target.text;

        $.ajax({
            type: "get",
            url: "/?currentPageIndex=" + NextPageIndex + "&SearchBy=" + SearchBy + "&SearchValue=" + SearchValue,
            contentType: "html",
            success: function (response) {
                $('#Data').html(jQuery(response).find('#Data').html());
                $('#Pagination').html(jQuery(response).find('#Pagination').html());
                history.pushState('', '', this.url);
            }
        })
    }
    )
})
        
        