(function () {
    $(document).ready(function () {
        $.ajax({
            url: 'http://localhost:29796/api/restaurant/query?Page=1&Size=99',
            type: "GET",
            dataType: "json",
            success: function (data) {
                var restaurants = $.map(data.items, function (item) {
                    return item.name;
                });
                $("#restaurant").autocomplete({
                    source: restaurants,
                    minLength: 1
                });
            },
            error: function (response) {
                alert(response.responseText);
            }
        });
    });
}());