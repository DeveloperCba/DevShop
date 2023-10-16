$(function () {
    $(window).on('beforeunload', function () {
        var img = '';
        var gif = $("#loading-pages").attr('src');
        var result = bowser.getParser(window.navigator.userAgent);

        if (result.parsedResult.browser.name == 'Firefox')
            img = `<img class="img-fluid" src=".${gif}" style="max-width: 50%; height: auto;">`;
        else {
            img = '<img class="custom-title-img">';
        }

        Swal.fire({
            title: img,
            background: `none`,
            customClass: {
                popup: 'custom-popup',
                title: 'custom-title',
                loader: 'custom-loader',
                actions: 'custom-actions'
            },
            allowOutsideClick: false,
            allowEscapeKey: false,
            didOpen: () => {
                swal.showLoading()
            }
        })
    });

    $(window).on('load', function () {
        Swal.close();
    });
});