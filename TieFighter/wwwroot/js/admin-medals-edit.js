﻿$(function () {
    $("#FileLocation").on("change", function (evt) {
        if (this.files && this.files[0]) {
            var reader = new FileReader();

            reader.onload = function (e) {
                $("#newImagePreview").attr('src', e.target.result);
            }

            reader.readAsDataURL(this.files[0]);
        }
    });
});