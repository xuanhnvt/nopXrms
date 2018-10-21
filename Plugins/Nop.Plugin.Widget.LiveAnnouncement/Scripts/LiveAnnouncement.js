$(function () {
    var connection = new signalR.HubConnectionBuilder()
        .withUrl('/announcement')
        .build();

    connection.on('send', data => {
        showannouncement(data);
    });

    function start() {
        connection.start().catch(function (err) {
            setTimeout(function () {
                start();
            }, 100000);
        });
    }

    connection.onclose(function () {
        start();
    });

    start();
});

function showannouncement(announcemant) {
    if (announcemant) {
        toastr.options = {
            "closeButton": true,
            "debug": false,
            "newestOnTop": false,
            "progressBar": false,
            "positionClass": "toast-bottom-right",
            "preventDuplicates": false,
            "onclick": null,
            "showDuration": 300,
            "hideDuration": 10000,
            "timeOut": 100000,
            "extendedTimeOut": 20000,
            "showEasing": "swing",
            "hideEasing": "linear",
            "showMethod": "fadeIn",
            "hideMethod": "fadeOut"
        };

        tostView = '<div class="item announcemantToast" style="position: relative;min-height: 75px;">' + announcemant + '</div>'
        toastr["info"](tostView);
        $('.toast-info').css("background-color", "#008080");


        toastr.options.onclick = function () {
            $("html, body").animate(
                { scrollTop: 0 },
                1000);
        }

        $(".toast").click(function () {
            $("html, body").animate(
                { scrollTop: 0 },
                1000);
        });

        $(".toast-info").click(function () {
            $("html, body").animate(
                { scrollTop: 0 },
                1000);
        });

        toastr.options = {
            onclick: function () {
                $("html, body").animate(
                    { scrollTop: 0 },
                    1000);
            }
        }

        $(".announcemantToast").on("click", function () {
            $("html, body").animate(
                { scrollTop: 0 },
                1000);
        });
    }
}
