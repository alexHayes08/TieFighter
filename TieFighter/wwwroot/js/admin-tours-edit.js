$(function () {
    var dt = setupDataTable("#missionTable", "#selectAll", "#deleteSelected");
    var previousValue = Number($("#Tour_Position").val());
    var currentTourId = $("#tourId").val();
    $("#Tour_Position").on("keyup", function () {
        function updateTable (arrOfTours) {
            var tableBody = $("#toursWithConflictingPositions tbody");

            // Clear out all rows except last one
            tableBody.find("tr:not(\":last-child\")").remove();

            // Attach new roles
            for (var tour of arrOfTours) {
                if (tour.tourId != currentTourId) {
                    var row = $("#conflictingPositionTableRowTemplate")[0]
                        .cloneNode(true)
                        .content;

                    var $aTag = $(row).find("a");
                    var $divTag = $(row).find("div");

                    $aTag.attr("href", `window.origin/Admin/Tours/Edit/${tour.tourId}`);
                    $aTag.html(tour.tourName);

                    $divTag.html(tour.position);

                    tableBody.prepend(row);
                }
            }
        }

        var $this = $(this);
        var value = Number($(this).val());
        if (value != "" && value != previousValue || value == 0)
        {
            XMLHttpRequestPromise("POST", `${window.location.origin}/Admin/Tours/ToursWithSamePosition/${value}`)
                .then(function (response) {
                    var jsonRes = JSON.parse(response);
                    previousValue = value;
                    console.log(jsonRes.results);

                    updateTable(jsonRes.results);

                }).catch(function (error) {
                    console.log(error);
                    previousValue = value;
                });
        }
    });

    $("#updateEntity").on("click", function () {

    })
});