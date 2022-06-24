﻿_url = '';

$(function () {
    _url.get_r_statistic_displayData = common.appPath + '/r_statistic/get_r_statistic_displayData';
    setValidation();
    addEvents();
    bind();
});

function bind() {
    window.chartColors = {
        red: 'rgb(255, 99, 132)',
        orange: 'rgb(255, 159, 64)',
        yellow: 'rgb(255, 205, 86)',
        green: 'rgb(75, 192, 192)',
        blue: 'rgb(54, 162, 235)',
        purple: 'rgb(153, 102, 255)',
        grey: 'rgb(231,233,237)',
        drak: 'rgb(0,0,0)'
    };

    var randomScalingFactor = function () {
        return (Math.random() > 0.5 ? 1.0 : 1.0) * Math.round(Math.random() * 100);
    };

    var line1 = [randomScalingFactor(), randomScalingFactor(), randomScalingFactor(), randomScalingFactor(),
    randomScalingFactor(), randomScalingFactor(), randomScalingFactor(), randomScalingFactor(),
    randomScalingFactor(), randomScalingFactor(), randomScalingFactor(), randomScalingFactor(),
    ];
    var line2 = [randomScalingFactor(), randomScalingFactor(), randomScalingFactor(), randomScalingFactor(),
    randomScalingFactor(), randomScalingFactor(), randomScalingFactor(), randomScalingFactor(),
    randomScalingFactor(), randomScalingFactor(), randomScalingFactor(), randomScalingFactor(),
    ];
    var line3 = [randomScalingFactor(), randomScalingFactor(), randomScalingFactor(), randomScalingFactor(),
    randomScalingFactor(), randomScalingFactor(), randomScalingFactor(), randomScalingFactor(),
    randomScalingFactor(), randomScalingFactor(), randomScalingFactor(), randomScalingFactor(),
    ];
    var line4 = [randomScalingFactor(), randomScalingFactor(), randomScalingFactor(), randomScalingFactor(),
    randomScalingFactor(), randomScalingFactor(), randomScalingFactor(), randomScalingFactor(),
    randomScalingFactor(), randomScalingFactor(), randomScalingFactor(), randomScalingFactor(),
    ];
    var line5 = [randomScalingFactor(), randomScalingFactor(), randomScalingFactor(), randomScalingFactor(),
    randomScalingFactor(), randomScalingFactor(), randomScalingFactor(), randomScalingFactor(),
    randomScalingFactor(), randomScalingFactor(), randomScalingFactor(), randomScalingFactor(),
    ];
    var line6 = [randomScalingFactor(), randomScalingFactor(), randomScalingFactor(), randomScalingFactor(),
    randomScalingFactor(), randomScalingFactor(), randomScalingFactor(), randomScalingFactor(),
    randomScalingFactor(), randomScalingFactor(), randomScalingFactor(), randomScalingFactor(),
    ];
    var line7 = [randomScalingFactor(), randomScalingFactor(), randomScalingFactor(), randomScalingFactor(),
    randomScalingFactor(), randomScalingFactor(), randomScalingFactor(), randomScalingFactor(),
    randomScalingFactor(), randomScalingFactor(), randomScalingFactor(), randomScalingFactor(),
    ];
    var line8 = [randomScalingFactor(), randomScalingFactor(), randomScalingFactor(), randomScalingFactor(),
    randomScalingFactor(), randomScalingFactor(), randomScalingFactor(), randomScalingFactor(),
    randomScalingFactor(), randomScalingFactor(), randomScalingFactor(), randomScalingFactor(),
    ];

    var MONTHS = ["2022年1月26日", "2022年1月27日", "2022年1月28日", "2022年1月29日", "2022年1月30日", "2022年1月31日", "2022年2月1日",
        "2022年2月2日", "2022年2月3日", "2022年2月4日", "2022年2月5日", "2022年2月6日"
    ];
    var config = {
        type: 'line',
        data: {
            labels: MONTHS,
            datasets: [{
                label: "詳細査定数",
                backgroundColor: window.chartColors.red,
                borderColor: window.chartColors.red,
                data: line1,
                fill: false,
                lineTension: 0,
            }, {
                label: "TOP5選出数",
                fill: false,
                lineTension: 0,
                backgroundColor: window.chartColors.blue,
                borderColor: window.chartColors.blue,
                data: line2,
            }, {
                label: "成約数",
                fill: false,
                lineTension: 0,
                backgroundColor: window.chartColors.orange,
                borderColor: window.chartColors.orange,
                data: line3,
            }, {
                label: "売主辞退数",
                fill: false,
                lineTension: 0,
                backgroundColor: window.chartColors.purple,
                borderColor: window.chartColors.purple,
                data: line5,
            }, {
                label: "買主辞退数",
                fill: false,
                lineTension: 0,
                backgroundColor: window.chartColors.green,
                borderColor: window.chartColors.green,
                data: line4,
            }]
        },
        options: {
            responsive: true,
            title: {
                display: true,
                text: '分析レポート'
            },
            tooltips: {
                mode: 'index',
                intersect: false,
            },
            hover: {
                mode: 'nearest',
                intersect: true
            },
            scales: {
                xAxes: [{
                    display: true,
                    scaleLabel: {
                        display: true,
                        labelString: '指定された範囲'
                    }
                }],
                yAxes: [{
                    display: true,
                    scaleLabel: {
                        display: true,
                    },
                }]
            }
        }
    };

    var ctx = document.getElementById("canvas").getContext("2d");
    var myLine = new Chart(ctx, config);
}

function setValidation() {
    $('.chk')
        .addvalidation_errorElement("#CheckBoxError")
        .addvalidation_checkboxlenght(); //E112

    $('#StartDate')
        .addvalidation_errorElement("#errorStartDate")
        .addvalidation_reqired()
        .addvalidation_datecheck() //E108

    $('#EndDate')
        .addvalidation_errorElement("#errorEndDate")
        .addvalidation_reqired()
        .addvalidation_datecheck() //E108
        .addvalidation_datecompare(); //E111
}

function addEvents() {
    common.bindValidationEvent('#form1', '');

    $('#StartDate, #EndDate').on('change', function () {
        const $this = $(this), $start = $('#StartDate').val(), $end = $('#EndDate').val();
        if (!common.checkValidityInput($this)) {
            return false;
        }
        let model = {
            StartDate: $start,
            EndDate: $end
        };
        if (model.StartDate && model.EndDate) {
            if (model.StartDate < model.EndDate) {
                $("#StartDate").hideError();
                $("#EndDate").hideError();
                $("#EndDate").focus();
                return;
            }
        }
    });

    $('.chk').on('change', function () {
        this.value = this.checked ? 1 : 0;
        if ($("input[type=checkbox]:checked").length > 0) {
            $('.form-check-input').hideError();
        }
    }).change();

    $('.rdo').on('change', function () {
        if ($('input[name=ryudo]:checked', '#form1').val() == '3') {
            $('#StartDate').prop('type', 'month');
            $('#StartDate').prop('max', '2999-12');
            $('#EndDate').prop('type', 'month');
            $('#EndDate').prop('max', '2999-12');
        }
        else {
            $('#StartDate').prop('type', 'date');
            $('#StartDate').prop('max', '2999-12-31');
            $('#EndDate').prop('type', 'date');
            $('#EndDate').prop('max', '2999-12-31');
        }
    });

    $('#btnThisMonth').on('click', function () {
        $('#month').attr('checked', true);
        $('#StartDate').prop('type', 'month');
        $('#StartDate').prop('max', '2999-12');
        $('#EndDate').prop('type', 'month');
        $('#EndDate').prop('max', '2999-12');

        let today = common.getToday().slice(0, 7);
        let firstDay = common.getFirstDayofMonth().slice(0, 7);
        $('#StartDate').val(firstDay);
        $('#EndDate').val(today);
    });
    $('#btnLastMonth').on('click', function () {
        $('#month').attr('checked', true);
        $('#StartDate').prop('type', 'month');
        $('#StartDate').prop('max', '2999-12');
        $('#EndDate').prop('type', 'month');
        $('#EndDate').prop('max', '2999-12');

        let firstdaypremonth = common.getFirstDayofPreviousMonth().slice(0, 7);
        let lastdaypremonth = common.getLastDayofPreviousMonth().slice(0, 7);
        $('#StartDate').val(firstdaypremonth);
        $('#EndDate').val(lastdaypremonth);
    });

    $('#btnDisplay').on('click', function () {
        $form = $('#form1').hideChildErrors();
        if (!common.checkValidityOnSave('#form1')) {
            $form.getInvalidItems().get(0).focus();
            return false;
        }
        $('#canvas').empty();
        var model = {
            dac_route: $('#dac_route').val(),
            dac_apartment: $('#dac_apartment').val(),
            top5_route: $('#top5_route').val(),
            top5_apartment: $('#top5_apartment').val(),
            contracts_route: $('#contracts_route').val(),
            contracts_apartment: $('#contracts_apartment').val(),
            bd_route: $('#bd_route').val(),
            bd_apartment: $('#bd_apartment').val(),
            sd_route: $('#sd_route').val(),
            sd_apartment: $('#sd_apartment').val(),
            ryudo: $('input[name=ryudo]:checked', '#form1').val(),
            StartDate: $('#StartDate').val(),
            EndDate: $('#EndDate').val(),
        }
        get_r_statistic_Data(model, $form);
    });
}

function get_r_statistic_Data(model, $form) {
    common.callAjaxWithLoading(_url.get_r_statistic_displayData, model, this, function (result) {
        if (result && result.isOK) {
            bind_r_statistic_Data(result.data);
        }
        if (result && !result.isOK) {
            const errors = result.data;
            for (key in errors) {
                const target = document.getElementById(key);
                $(target).showError(errors[key]);
                $form.getInvalidItems().get(0).focus();
            }
        }
    });
}

function bind_r_statistic_Data(result) {

}