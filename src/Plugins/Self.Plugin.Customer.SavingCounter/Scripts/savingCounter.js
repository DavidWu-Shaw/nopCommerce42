(function () {

    var savingCounterManager = {

        init: function (settings) {
            console.log(settings);
            // Register click event for close button
            $('#savings-counter-close').click(function () {
                // Hide the saving counter
                $('#savings-counter-pannel').remove();
                // Stop the refresh timer
                clearInterval(savingCounterRefreshTimer);
                // Set cookie to make sure hide for 7 days by setting disableCounter=1
                SavingCounterManager.setCookie('disableCounter', '1', 7);
            });

            // Check cookie for disableCounter
            var disableCounter = SavingCounterManager.getCookie('disableCounter');
            if (disableCounter == '1') {
                // Hide the saving counter
              $('#savings-counter-pannel').remove();
                return;
            }

            // Set timer for refresh the counter
            var savingCounterRefreshTimer = setInterval(SavingCounterManager.UpdateTotalSavings, settings.millisecondsOfCounterRefresh);
        },

        UpdateTotalSavings: function() {
            $.ajax({
                type: "GET",
                cache: false,
                url: "/savingcounter/gettotalsavings"
            }).done(function (result) {
                $('#total-savings').text(result);
            })
        },

        setCookie: function (cname, cvalue, exdays) {
            var d = new Date();
            d.setTime(d.getTime() + (exdays * 24 * 60 * 60 * 1000));
            var expires = "expires=" + d.toUTCString();
            document.cookie = cname + "=" + cvalue + ";" + expires + ";path=/";
        },

        getCookie: function (cname) {
            var name = cname + "=";
            var ca = document.cookie.split(';');
            for (var i = 0; i < ca.length; i++) {
                var c = ca[i];
                while (c.charAt(0) == ' ') {
                    c = c.substring(1);
                }
                if (c.indexOf(name) == 0) {
                    return c.substring(name.length, c.length);
                }
            }
            return "";
        }
    };

    window.SavingCounterManager = savingCounterManager;
}());