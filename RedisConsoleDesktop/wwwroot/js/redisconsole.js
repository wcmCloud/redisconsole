﻿// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

//the bottom padding of the grids

const GridBottomPadding = 230;
const NotificationChannel = "rcd-info";
const MessageDelimiter = "###";

function sendAppNotification(title, message) {
    sendNotification(NotificationChannel, title, message);
}

function sendNotification(channel, title, message) {
    //renderer.js
    const ipc = require('electron').ipcRenderer;
    ipc.send(channel, title + MessageDelimiter + message);

}


function collectFormData() {
    var fdata = new FormData();
    var formdata = $('form').serializeArray();
    $.each(formdata, function (key, input) {
        fdata.append(input.name, input.value);
    });
    //for (var key of fdata.entries()) {
    //    console.log(key[0] + ', ' + key[1]);
    //}

    return fdata;
}


function kendoMultiselectData(controlName) {
    var multiselect = $("#" + controlName).data("kendoMultiSelect");
    var selectedData = [];
    var totitems = multiselect.value();
    for (var i = 0; i < totitems.length; i++) {
        selectedData.push(totitems[i]);
    }
    return selectedData.toString();
}

function quickPostForm(data) {
    var manJSCmon = location.pathname.split("/");
    var controller = ((manJSCmon.length > 1) && (manJSCmon[1] != "")) ? manJSCmon[1] : "";
    var action = ((manJSCmon.length > 1) && (manJSCmon[2] != "")) ? manJSCmon[2] : "";;

    postForm(action, controller, data);
}

function postForm(action, controller, data) {

    $.ajax({
        url: "/" + controller + "/" + action,
        type: 'POST',
        contentType: 'application/x-www-form-urlencoded',
        //contentType: 'multipart/form-data',
        contentType: false,
        //enctype: 'multipart/form-data',
        processData: false,
        data: data,
        success: function (data) {
            //send notification
            //var notfmsg = controller + " " + action + " success";
            //sendAppNotification(notfmsg, notfmsg);
        },
        error: function (ex, status) {
            console.log("/" + controller + "/" + action + " Error in manipulating the record" + ex);
            //send notification
            var notfmsg = controller + " " + action + " FAILED";
            sendAppNotification(notfmsg, notfmsg + " " + status.code + " exception: " + ex);
        }

    });
}