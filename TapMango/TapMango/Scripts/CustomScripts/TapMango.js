var tapMango = (function () {
    return {
        init: function () {
            var elements = {
                banner: $("#banner-stage"),
                status: $("#status"),
                bannerSelector: $("#banner-selector"),
                bannerSelectorButton: $("#banner-selector .banner-selector-container > a"),
                waterButton: $("#banner-stage .banner-stage-container .banner-overlay-left > a"),
                stopButton: $("#banner-stage .banner-stage-container .banner-overlay-right > a"),
            };

            elements.status.slick({
                arrows: false,
                speed: 300,
                slidesToShow: 1,
                asNavFor: '#banner-stage'
            });

            elements.banner.slick({
                dots: true,
                infinite: true,
                speed: 300,
                slidesToShow: 1,
                asNavFor: '#status'
            });

            elements.banner.on('afterChange', function (event, slick, currentSlide, nextSlide) {
                sessionStorage.setItem('bannerSlideIndex', currentSlide);
            });

            if (sessionStorage.getItem("bannerSlideIndex") !== null) {
                elements.banner.slick('slickGoTo', sessionStorage.getItem("bannerSlideIndex"));
            }

            elements.bannerSelector.slick({
                infinite: true,
                speed: 300,
                slidesToShow: 5,
            });

            elements.bannerSelectorButton.on("click", function () {
                var $this = $(this);

                elements.banner.slick('slickGoTo', $this.closest(".slick-slide").data("slickIndex"))
            });

            elements.waterButton.on("click", function () {
                var $this = $(this);

                $.ajax({
                    type: "POST",
                    url: "/main/startwatering",
                    data: { tapMangoPlantId: $this.closest(".banner-stage-container").data("tapmangoplantid") },
                    success: function (data) {
                        alert(data.Message);

                        if (data.Success) {
                            location.reload();
                        }
                    }
                });

                return false;
            });

            elements.stopButton.on("click", function () {
                var $this = $(this);

                $.ajax({
                    type: "POST",
                    url: "/main/stopwatering",
                    data: { tapMangoPlantId: $this.closest(".banner-stage-container").data("tapmangoplantid") },
                    success: function (data) {
                        alert(data.Message);

                        if (data.Success) {
                            location.reload();
                        }
                    }
                });

                return false;
            });

            $("#status .past-due").each(function () {
                var $this = $(this)

                $("#banner-stage-" + $this.closest(".status-container").data("tapmangoplantid")).addClass("show-warning");
                $("#banner-selector-" + $this.closest(".status-container").data("tapmangoplantid")).addClass("show-warning");
            });

            // 6 hours
            var nextEventTime = 21600001;
            $("#status .status-input").each(function () {
                // find smallest 
                var $this = $(this);

                if (!$this.hasClass("status-stopped-watering")) {
                    var wateringsessiontimeleft = parseFloat($this.data("wateringsessiontimeleft"));
                    var wateragaintimeleft = parseFloat($this.data("wateragaintimeleft"));

                    if (nextEventTime > wateringsessiontimeleft && wateringsessiontimeleft != 0)
                        nextEventTime = wateringsessiontimeleft;
                    if (nextEventTime > wateragaintimeleft && wateragaintimeleft != 0)
                        nextEventTime = wateragaintimeleft;
                }
            })

            if (nextEventTime != 21600001) {
                setInterval(function () {
                    location.reload();
                }, nextEventTime);
            }
        }
    };
})();