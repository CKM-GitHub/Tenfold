const _url = {};

$(function () {
    _url.get_r_statistic_displayData = common.appPath + '/r_statistics/get_r_statistic_displayData';
    setValidation();
    addEvents();
});

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
        $('#day').prop('checked', true);
        $('#StartDate').prop('type', 'date');
        $('#StartDate').prop('max', '2999-12-31');
        $('#EndDate').prop('type', 'date');
        $('#EndDate').prop('max', '2999-12-31');

        let today = common.getToday();
        let firstDay = common.getFirstDayofMonth();
        $('#StartDate').val(firstDay);
        $('#EndDate').val(today);
    });
    $('#btnLastMonth').on('click', function () {
        $('#day').prop('checked', true);
        $('#StartDate').prop('type', 'date');
        $('#StartDate').prop('max', '2999-12-31');
        $('#EndDate').prop('type', 'date');
        $('#EndDate').prop('max', '2999-12-31');

        let firstdaypremonth = common.getFirstDayofPreviousMonth();
        let lastdaypremonth = common.getLastDayofPreviousMonth();
        $('#StartDate').val(firstdaypremonth);
        $('#EndDate').val(lastdaypremonth);
    });

    $('#btnDisplay').on('click', function () {
        $form = $('#form1').hideChildErrors();
        if (!common.checkValidityOnSave('#form1')) {
            $form.getInvalidItems().get(0).focus();
            return false;
        }
        if (!check_DateDifference()) {
            $form.getInvalidItems().get(0).focus();
            return false;
        }

        const formatDate = () => {
            var aggregationtime = '';
            let d = new Date();
            let month = (d.getMonth() + 1).toString();
            let day = d.getDate().toString();
            let year = d.getFullYear();
            if (month.length < 2) {
                month = '0' + month;
            }
            if (day.length < 2) {
                day = '0' + day;
            }
            aggregationtime = [year, month, day].join('/') + ' ' + d.getHours() + ':' + d.getMinutes() + ':' + d.getSeconds();
            return aggregationtime;
        };

        $('#canvas').remove();
        $('.cap-repotcanvas').append('<canvas id="canvas"><canvas>');
        const d = new Date();
        $('#t_aggregationtime').text('集計時間 ' + formatDate());
        $('#b_aggregationtime').text('集計時間 ' + formatDate());
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
            StartDate: $('#sdate').text(),
            EndDate: $('#edate').text(),
        }
        get_r_statistic_Data(model, $form);
    });
}

function check_DateDifference() {
    debugger;
    var sdate = '';
    var edate = '';
    if ($('input[name=ryudo]:checked', '#form1').val() == 1) {
        sdate = new Date($('#StartDate').val());
        edate = new Date($('#EndDate').val());
        if ((Math.ceil(Math.abs(edate - sdate) / (1000 * 60 * 60 * 24))) + 1 > 30) {
            $('#EndDate').showError(common.getMessage('E117'));
            return false;
        }
        else {
            $('#sdate').text($('#StartDate').val());
            $('#edate').text($('#EndDate').val());
        }
    }
    else if ($('input[name=ryudo]:checked', '#form1').val() == 2) {
        sdate = new Date($('#StartDate').val());
        edate = new Date($('#EndDate').val());
        if (Math.abs(Math.round(((edate - sdate) / 1000) / (60 * 60 * 24 * 7))) > 30) {
            $('#EndDate').showError(common.getMessage('E118'));
            return false;
        }
        else {
            $('#sdate').text($('#StartDate').val());
            $('#edate').text($('#EndDate').val());
        }
    }
    else if ($('input[name=ryudo]:checked', '#form1').val() == 3) {
        sdate = new Date($('#StartDate').val() + '-01');
        edate = new Date($('#EndDate').val() + '-' + new Date(parseInt($('#EndDate').val().slice(0, 4)), parseInt($('#EndDate').val().slice(5, 7)), 0).getDate());
        if ((((edate.getFullYear() - sdate.getFullYear()) * 12) + edate.getMonth() - sdate.getMonth()) > 24) {
            $('#EndDate').showError(common.getMessage('E119'));
            return false;
        }
        else {
            $('#sdate').text($('#StartDate').val() + '-01');
            $('#edate').text($('#EndDate').val() + '-' + new Date(parseInt($('#EndDate').val().slice(0, 4)), parseInt($('#EndDate').val().slice(5, 7)), 0).getDate());
        }
    }
    return true;
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
    var line1 = '', line2 = '', line3 = '', line4 = '', line5 = '', DisplayDate = '';
    var data = JSON.parse(result);
    if (data.length > 0) {
        for (var i = 0; i < data.length; i++) {
            if (data[i]["NO"] == '1')
                line1 = data[i]["IDList"].split(',');
            else if (data[i]["NO"] == '2')
                line2 = data[i]["IDList"].split(',');
            else if (data[i]["NO"] == '3')
                line3 = data[i]["IDList"].split(',');
            else if (data[i]["NO"] == '4')
                line4 = data[i]["IDList"].split(',');
            else if (data[i]["NO"] == '5')
                line5 = data[i]["IDList"].split(',');
        }
        DisplayDate = data[0]["DisplayDate"].split(',');

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

        var config = {
            type: 'line',
            data: {
                labels: DisplayDate,
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
}