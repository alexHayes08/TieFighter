$(function () {
    $(function () {
        $(document).ready(function () {
            $('#table').DataTable();
        });
    });

    $("#selectAll").on("change", function () {

        //console.log($(this).prop("checked"));

        if ($(this).prop("checked")) {

            // Select all medals
            $("#table tbody tr input[type='checkbox']").prop("checked", true);

        } else {

            // Deselect all medals
            $("#table tbody tr input[type='checkbox']").prop("checked", false);
        }
    });
});