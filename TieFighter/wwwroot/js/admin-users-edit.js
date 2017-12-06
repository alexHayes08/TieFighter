﻿$(function () {
    $("#updateEntity").on("click", function () {
        var $inputs = $("body main input,body main textarea,body main button");

        setInputsTo($inputs, false);
        pageLoading(true);

        function show() {
            setInputsTo($inputs, true);
            pageLoading(false);
        }

        var formData = new FormData($("#userInfoForm")[0])

        // Append roles
        var userRoles = [];
        $("#userRoles input").each(function () {
            if ($(this).prop("checked")) {
                userRoles.push($(this).attr("name"));
            }
        });

        formData.append("Roles", userRoles.join(","));

        XMLHttpRequestPromise("POST", window.location.href, formData, false)
            .then(function (response) {
                var res = JSON.parse(response);
                if (!res.succeeded) {
                    throw new Error(res.Error);
                }
                show();
                new jBox('Notice', {
                    content: "Successfully updated.",
                    color: "green"
                }).open();
            }).catch(function (error) {
                console.log(error);
                show();
                new jBox('Notice', {
                    content: "Failed to update!",
                    color: "red"
                }).open();
            });
    });
});