/// <reference path="jquery-1.7.1.js" />
/// <reference path="jquery.validate.js" />

$(function () {

    var getValidationSummaryErrors = function ($form) {
        // We verify if we created it beforehand
        var errorSummary = $form.data('validation-summary-errors');
        if (!errorSummary) {
            errorSummary = $('<div class="validation-summary-errors"><span>Please correct the errors and try again.</span><ul></ul></div>')
                .insertBefore($form);

            // Remember that we created it
            $form.data('validation-summary-errors', errorSummary);
        }

        return errorSummary;
    };

    var formSubmitHandler = function (e) {
        var $form = $(this);

        // We check if jQuery.validator exists on the form
        if (!$form.valid || $form.valid()) {
            $.post($form.attr('action'), $form.serializeArray())
                .done(function (json) {
                    json = json || {};

                    // In case of success, we redirect to the provided URL or the same page.
                    if (json.success) {
                        location = json.redirect || location.href;
                    } else if (json.errors) {
                        var errorSummary = getValidationSummaryErrors($form);

                        var items = $.map(json.errors, function (error) {
                            return '<li>' + error + '</li>';
                        }).join('');

                        var ul = errorSummary
                            .find('ul')
                            .empty()
                            .append(items);
                    }
                });
        }

        // Prevent the normal behavior since we opened the dialog
        e.preventDefault();
    };

    var loadAndShowDialog = function (link, url) {
        var separator = url.indexOf('?') >= 0 ? '&' : '?';

        // Load the dialog with the content=1 QueryString in order to get a PartialView
        $.get(url + separator + 'content=1')
            .done(function (content) {
                $('<div class="modal-popup">' + content + '</div>')
                    .hide() // Hide the dialog for now so we prevent flicker
                    .appendTo(document.body)
                    .filter('div') // Filter for the div tag only, script tags could surface
                    .dialog({ // Create the jQuery UI dialog
                        title: link.data('dialog-title'),
                        modal: true,
                        resizable: true,
                        draggable: true,
                        width: link.data('dialog-width') || 300
                    })
                    .find('form') // Attach logic on forms
                        .submit(formSubmitHandler)
                    .end();
            });
    };

    var links = ['popupLink'];

    $.each(links, function (i, popupclass) {
        $('.' + popupclass).click(function (e) {
            var link = $(this),
        url = link.attr('href');

            loadAndShowDialog(link, url);

            // Prevent the normal behavior since we use a dialog
            e.preventDefault();
        });
    });
});