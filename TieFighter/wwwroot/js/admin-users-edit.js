$(function () {
    $("#createRole").on("click", function () {
        var roleName = $("#newRoleName").val();
        var existingRoleNames = [];
        $("#userRoles li").each(function () {
            existingRoleNames.push(this.innerText.trim());
        });

        var newRoleElContainer = document.createElement("li");
        var newRoleElLabel = document.createElement("label");
        var newRoleEl = document.createElement("input");

        newRoleEl.classList.add("custom-checkbox", "align-middle");
        newRoleEl.setAttribute("name", roleName);
        newRoleEl.setAttribute("type", "checkbox");

        newRoleElLabel.appendChild(newRoleEl);
        newRoleElContainer.appendChild(newRoleElLabel);

        newRoleElLabel.innerHTML += roleName;

        $("#userRoles").append(newRoleElContainer);
    });

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