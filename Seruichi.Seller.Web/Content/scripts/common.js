window.addEventListener('pageshow', function (event) {
    if (event.persisted) {
        window.location.reload();
    }
});

//in Layout.cshtml, the database values are set
const messageConst = {
    E101: '入力が必要です',
    E102: '必ず１つ選択してください',
    E103: '入力された値が正しくありません',
    E104: '入力できない文字です',
    E105: '入力できる桁数を超えています',
    E106: 'システム内部で必要な設定が足りていません',
    E107: '全角文字で入力してください',
    E108: '正しい日付を入力してください',
    E109: 'パスワードが違います',
    E110: '入力すべき文字数を満たしていません',
    E111: '日付の大小が不正です',
    E112: '１つ以上選択してください',

    E201: '査定サービスの対象外地域です',
    E202: 'メールアドレスが入力されていません',
    E203: '既に登録済のメールアドレスです',
    E204: 'メールアドレスを正しく入力してください',
    E205: 'パスワードが入力されていません',
    E206: 'メールアドレスとパスワードの組合せが正しくありません',
    E207: 'この情報での登録はありません',
    E213: 'セルイチを退会します」のチェックボックスがオフです。',
}

const commonApiUrl = {
    getMessage: "/api/CommonApi/GetMessage",
    getBuildingAge: "/api/CommonApi/GetBuildingAge",
    getDropDownListItemsOfPrefecture: "/api/CommonApi/GetDropDownListItemsOfPrefecture",
    getDropDownListItemsOfCity: "/api/CommonApi/GetDropDownListItemsOfCity",
    getDropDownListItemsOfTown: "/api/CommonApi/GetDropDownListItemsOfTown",
    getDropDownListItemsOfLine: "/api/CommonApi/GetDropDownListItemsOfLine",
    getDropDownListItemsOfStation: "/api/CommonApi/GetDropDownListItemsOfStation",
    checkBirthday: "/api/CommonApi/CheckBirthday",
    getNearestStations: "/api/CommonApi/GetNearestStations",
}

const regexPattern = {
    doublebyte: "[^\x01-\x7E\uFF61-\uFF9F]",
    singlebyte: "^[a-zA-Z0-9!-/:-@¥[-`{-~]*$",
    singlebyte_number: "^[0-9]+$",
    singlebyte_numberAlpha: "^[0-9a-zA-Z]+$",
    moeney: "^[0-9,]+$",
    numeric: "^[0-9.]+$",
    doublebyteKana: "^[ァ-ヶー　]+$",
}

const kanaMap = {
    'ｶﾞ': 'ガ', 'ｷﾞ': 'ギ', 'ｸﾞ': 'グ', 'ｹﾞ': 'ゲ', 'ｺﾞ': 'ゴ',
    'ｻﾞ': 'ザ', 'ｼﾞ': 'ジ', 'ｽﾞ': 'ズ', 'ｾﾞ': 'ゼ', 'ｿﾞ': 'ゾ',
    'ﾀﾞ': 'ダ', 'ﾁﾞ': 'ヂ', 'ﾂﾞ': 'ヅ', 'ﾃﾞ': 'デ', 'ﾄﾞ': 'ド',
    'ﾊﾞ': 'バ', 'ﾋﾞ': 'ビ', 'ﾌﾞ': 'ブ', 'ﾍﾞ': 'ベ', 'ﾎﾞ': 'ボ',
    'ﾊﾟ': 'パ', 'ﾋﾟ': 'ピ', 'ﾌﾟ': 'プ', 'ﾍﾟ': 'ペ', 'ﾎﾟ': 'ポ',
    'ｳﾞ': 'ヴ', 'ﾜﾞ': 'ヷ', 'ｦﾞ': 'ヺ',
    'ｱ': 'ア', 'ｲ': 'イ', 'ｳ': 'ウ', 'ｴ': 'エ', 'ｵ': 'オ',
    'ｶ': 'カ', 'ｷ': 'キ', 'ｸ': 'ク', 'ｹ': 'ケ', 'ｺ': 'コ',
    'ｻ': 'サ', 'ｼ': 'シ', 'ｽ': 'ス', 'ｾ': 'セ', 'ｿ': 'ソ',
    'ﾀ': 'タ', 'ﾁ': 'チ', 'ﾂ': 'ツ', 'ﾃ': 'テ', 'ﾄ': 'ト',
    'ﾅ': 'ナ', 'ﾆ': 'ニ', 'ﾇ': 'ヌ', 'ﾈ': 'ネ', 'ﾉ': 'ノ',
    'ﾊ': 'ハ', 'ﾋ': 'ヒ', 'ﾌ': 'フ', 'ﾍ': 'ヘ', 'ﾎ': 'ホ',
    'ﾏ': 'マ', 'ﾐ': 'ミ', 'ﾑ': 'ム', 'ﾒ': 'メ', 'ﾓ': 'モ',
    'ﾔ': 'ヤ', 'ﾕ': 'ユ', 'ﾖ': 'ヨ',
    'ﾗ': 'ラ', 'ﾘ': 'リ', 'ﾙ': 'ル', 'ﾚ': 'レ', 'ﾛ': 'ロ',
    'ﾜ': 'ワ', 'ｦ': 'ヲ', 'ﾝ': 'ン',
    'ｧ': 'ァ', 'ｨ': 'ィ', 'ｩ': 'ゥ', 'ｪ': 'ェ', 'ｫ': 'ォ',
    'ｯ': 'ッ', 'ｬ': 'ャ', 'ｭ': 'ュ', 'ｮ': 'ョ',
    '｡': '。', '､': '、', 'ｰ': 'ー', '｢': '「', '｣': '」', '･': '・'
};

const common = {

    appPath: "", //set in Layout.cshtml

    getMessage: function getMessage(id) {
        let msg = messageConst[id];
        if (msg) return messageConst[id];

        common.callAjax(common.appPath + commonApiUrl.getMessage, { MessageID: id }, function (result) {
            if (result && result.isOK) {
                msg = result.data.MessageText1;
                messageConst[id] = msg;
                return msg;
            }
        });
    },

    querySerialize: function querySerialize(data) {
        const encode = window.encodeURIComponent;
        let key, value, type, i, max;
        let query = '';

        for (key in data) {
            value = data[key];
            type = typeof (value) === 'object' && value instanceof Array ? 'array' : typeof (value);
            switch (type) {
                case 'undefined':
                    // only key
                    query += key;
                    break;
                case 'array':
                    // array
                    for (i = 0, max = value.length; i < max; i++) {
                        query += key + '[]';
                        query += '=';
                        query += encode(value[i]);
                        query += '&';
                    }
                    query = query.substr(0, query.length - 1);
                    break;
                case 'object':
                    // hash
                    for (i in value) {
                        query += key + '[' + i + ']';
                        query += '=';
                        query += encode(value[i]);
                        query += '&';
                    }
                    query = query.substr(0, query.length - 1);
                    break;
                default:
                    query += key;
                    query += '=';
                    query += encode(value);
                    break;
            }
            query += '&';
        }
        query = query.substr(0, query.length - 1);
        return '?' + encodeURI(query);
    },

    redirectErrorPage: function redirectErrorPage(status) {
        let url;
        if (status == 400) url = "/Error/BadRequest";
        if (status == 401) url = "/Error/Unauthorized";
        if (status == 403) url = "/Error/Forbidden";
        if (status == 404) url = "/Error/NotFound";
        //if (status == 500) url = "/Error/InternalServerError";

        if (url) location.href = common.appPath + url;
    },

    callAjaxWithLoading: function callAjaxWithLoading(url, model, disableSelector, successCallback, failCallback) {
        common.showLoading(disableSelector);
        $.ajax({
            url: url,
            type: "POST",
            contentType: 'application/json; charset=utf-8',
            dataType: "json",
            data: JSON.stringify(model),
            headers: {
                RequestVerificationToken: $("#_RequestVerificationToken").val(),
            },
            async: true,
            timeout: 30000,
        }).done(function (data) {
            common.hideLoading(disableSelector);
            if (successCallback) {
                successCallback(data);
            }
        }).fail(function (XMLHttpRequest, status, e) {
            common.hideLoading(disableSelector);
            common.redirectErrorPage(XMLHttpRequest.status);
            if (failCallback) {
                failCallback();
            }
        });
    },
    callAjaxWithLoadingSync: function callAjaxWithLoadingSync(url, model, disableSelector, successCallback, failCallback) {
        common.showLoading(disableSelector);
        $.ajax({
            url: url,
            type: "POST",
            contentType: 'application/json; charset=utf-8',
            dataType: "json",
            data: JSON.stringify(model),
            headers: {
                RequestVerificationToken: $("#_RequestVerificationToken").val(),
            },
            async: false,
            timeout: 30000,
        }).done(function (data) {
            common.hideLoading(disableSelector);
            if (successCallback) {
                successCallback(data);
            }
        }).fail(function (XMLHttpRequest, status, e) {
            common.hideLoading(disableSelector);
            common.redirectErrorPage(XMLHttpRequest.status);
            if (failCallback) {
                failCallback();
            }
        });
    },
    callAjax: function callAjax(url, model, successCallback, failCallback) {
        $.ajax({
            url: url,
            type: "POST",
            contentType: 'application/json; charset=utf-8',
            //dataType: "json",
            data: JSON.stringify(model),
            headers: {
                RequestVerificationToken: $("#_RequestVerificationToken").val(),
            },
            async: true,
            timeout: 100000,
        }).done(function (data) {
            if (successCallback) {
                successCallback(data);
            }
        }).fail(function (XMLHttpRequest, status, e) {
            common.redirectErrorPage(XMLHttpRequest.status);
            if (failCallback) {
                failCallback();
            }
        });
    },

    callSubmit: function callSubmit(form, action) {
        const input = document.createElement('input');
        input.setAttribute('type', 'hidden');
        input.setAttribute('name', 'RequestVerificationToken');
        input.setAttribute('value', $("#_RequestVerificationToken").val());
        form.appendChild(input);

        form.method = "POST";
        form.action = action;
        form.submit();
        this.showLoading();
    },

    removeDropDownListItems: function removeDropDownListItems(selector, placeHolder) {
        const $ddl = $(selector);
        $ddl.children().remove();
        if (placeHolder || placeHolder === "") {
            $ddl.append('<option selected value="">' + placeHolder + '</option>');
        }
    },

    setDropDownListItems: function setDropDownListItems(selector, data, placeHolder, defaultval) {
        const $ddl = $(selector);
        this.removeDropDownListItems($ddl, placeHolder);

        let html = "";
        if (data && data.length > 0) {
            data.forEach(function (item) {
                html += '<option class="js-selectable" value=' + item.Value + '>' + item.DisplayText + '</option>';
            });
            $ddl.append(html);
        }

        if (defaultval) {
            $ddl.val(defaultval);
        }
    },

    showLoading: function showLoading(disableSelector) {
        $loadiong = $('<div id="loadiong"><div class="cv-spinner"><span class="spinner"></span></div></div>');
        $('body').append($loadiong);
        $('#loadiong').fadeIn(300);
        if (disableSelector) $(disableSelector).prop('disabled', true);
    },

    hideLoading: function hideLoading(disableSelector, delaytime) {
        setTimeout(function () {
            if (disableSelector) $(disableSelector).prop('disabled', false);
            $("#loadiong").fadeOut(300).remove();
        }, delaytime || 500);
    },

    birthdayCheck: function birthdayCheck(ctrl) {
        const $ctrl = $(ctrl);

        let inputValue = $ctrl.val();
        if (!inputValue) return;

        inputValue = common.replaceDoubleToSingle(inputValue);
        inputValue = inputValue.replace('年', '/').replace('月', '/').replace('日', '');
        inputValue = inputValue.replace(/-/g, '/');
        $ctrl.val(inputValue);

        const regex = new RegExp(regexPattern.singlebyte_number);
        if (!regex.test(inputValue.replace(/\//g, ''))) {
            $ctrl.showError(this.getMessage('E104'));
            return;
        }

        common.callAjax(common.appPath + commonApiUrl.checkBirthday, $ctrl.val(), function (result) {
            if (result) {
                if (result.data) $ctrl.val(result.data);
                if (result.message) {
                    $ctrl.showError(result.message.MessageText1);
                } else {
                    $ctrl.hideError();
                }
            }
        });
    },

    getStringByteCount: function getStringByteCount(str) {
        var blob = new Blob([str], { type: 'text/plain' });
        return blob.size;
    },

    replaceDoubleToSingle: function replaceDoubleToSingle(str) {
        //return str.replace(/[！-～]/g, function (s) {
        //    return String.fromCharCode(s.charCodeAt(0) - 0xFEE0);
        //});
        let returnValue = str.replace(/[！-～]/g, function (s) {
            return String.fromCharCode(s.charCodeAt(0) - 0xFEE0);
        });
        return returnValue.replace(/　/g, " ");
    },

    replaceSingleToDouble: function replaceSingleToDouble(str) {        
        //return str.replace(/[!-~]/g, function (s) {
        //    return String.fromCharCode(s.charCodeAt(0) + 0xFEE0);
        //});
        let returnValue = str.replace(/[!-~]/g, function (s) {
            return String.fromCharCode(s.charCodeAt(0) + 0xFEE0);
        });
        return returnValue.replace(/ /g, "　");
    },

    replaceSingleToDoubleKana: function replaceSingleToDoubleKana(str) {
        var reg = new RegExp('(' + Object.keys(kanaMap).join('|') + ')', 'g');
        return str
            .replace(reg, function (match) {
                return kanaMap[match];
            })
            .replace(/ﾞ/g, '゛')
            .replace(/ﾟ/g, '゜');
    },

    checkValidityInput: function checkValidityInput(ctrl) {
        const $ctrl = $(ctrl);
        const type = $ctrl.attr('type');
        const name = $ctrl.attr('name');

        const isRequired = $ctrl.attr("data-validation-required");
        const isRequiredNotAllowZero = $ctrl.attr("data-validation-not-allow-zero");
        const isSingleDoubleByte = $ctrl.attr("data-validation-singlebyte-doublebyte");
        const isDoubleByte = $ctrl.attr("data-validation-doublebyte");
        const isDoubleByteKana = $ctrl.attr("data-validation-doublebyte-kana");
        const isSingleByte = $ctrl.attr("data-validation-singlebyte");
        const isSingleByteNumber = $ctrl.attr("data-validation-singlebyte-number");
        const isSingleByteNumberAlpha = $ctrl.attr("data-validation-singlebyte-numberalpha");
        const isNumeric = $ctrl.attr("data-validation-numeric");
        const isMoney = $ctrl.attr("data-validation-money");
        const customValidation = $ctrl.attr("data-validation-custom");

        let inputValue = "";
        if (type === 'radio') {
            inputValue = $('input[name="' + name + '"]:radio:checked').val();
        } else {
            inputValue = $ctrl.val();
            if (inputValue) {
                inputValue = inputValue.trimEnd();
                $ctrl.val(inputValue);
            }
        }

        //replace text
        if (isSingleByte || isSingleByteNumber || isSingleByteNumberAlpha || isNumeric || isMoney) {
            inputValue = this.replaceDoubleToSingle(inputValue);
            $ctrl.val(inputValue);
        }

        if (isDoubleByte || isDoubleByteKana) {
            inputValue = this.replaceSingleToDouble(inputValue);
            inputValue = this.replaceSingleToDoubleKana(inputValue);
            $ctrl.val(inputValue);
        }

        //validation
        if (!isRequired
            && !isSingleDoubleByte
            && !isDoubleByte && !isDoubleByteKana
            && !isSingleByte && !isSingleByteNumber && !isSingleByteNumberAlpha
            && !isNumeric && !isMoney
            && !customValidation) return true;

        if (isRequired) {
            if (!inputValue) {
                if ($ctrl.prop("tagName") === 'SELECT') {
                    if ($ctrl.children('.js-selectable').length > 0) {
                        $ctrl.showError(this.getMessage('E102'));
                        return false;
                    }
                }
                else if (type === 'radio') {
                    $ctrl.showError(this.getMessage('E102'));
                    return false;
                }
                else {
                    $ctrl.showError(this.getMessage('E101'));
                    return false;
                }
            }
            if (isRequiredNotAllowZero && Number(inputValue) === 0) {
                $ctrl.showError(this.getMessage('E101'));
                return false;
            }
        }

        if (inputValue) {
            //if (isSingleDoubleByte) {
            //    const maxLength = $ctrl.attr('maxlength');
            //    if (maxLength) {
            //        const byteLength = this.getStringByteCount(inputValue);
            //        if (byteLength > parseInt(maxLength)) {
            //            $ctrl.showError(this.getMessage('E105'));
            //            return;
            //        }
            //    }
            //}

            if (isDoubleByte) {
                const regex = new RegExp(regexPattern.doublebyte);
                if (!regex.test(inputValue)) {
                    $ctrl.showError(this.getMessage('E107'));
                    return;
                }

                //const maxLength = $ctrl.attr('maxlength');
                //if (maxLength) {
                //    const byteLength = this.getStringByteCount(inputValue);
                //    if (byteLength > parseInt(maxLength)) {
                //        $ctrl.showError(this.getMessage('E105'));
                //        return;
                //    }
                //}
            }

            if (isDoubleByteKana) {
                const regex = new RegExp(regexPattern.doublebyteKana);
                if (!regex.test(inputValue)) {
                    $ctrl.showError(this.getMessage('E104'));
                    return;
                }

                //const maxLength = $ctrl.attr('maxlength');
                //if (maxLength) {
                //    const byteLength = this.getStringByteCount(inputValue);
                //    if (byteLength > parseInt(maxLength)) {
                //        $ctrl.showError(this.getMessage('E105'));
                //        return;
                //    }
                //}
            }

            if (isSingleByte) {
                const regex = new RegExp(regexPattern.singlebyte);
                if (!regex.test(inputValue)) {
                    $ctrl.showError(this.getMessage('E104'));
                    return;
                }
            }

            if (isSingleByteNumber) {
                const regex = new RegExp(regexPattern.singlebyte_number);
                if (!regex.test(inputValue)) {
                    $ctrl.showError(this.getMessage('E104'));
                    return;
                }
            }

            if (isSingleByteNumberAlpha) {
                const regex = new RegExp(regexPattern.singlebyte_numberAlpha);
                if (!regex.test(inputValue)) {
                    $ctrl.showError(this.getMessage('E104'));
                    return;
                }
            }

            if (isNumeric) {
                const regex = new RegExp(regexPattern.numeric);
                if (!regex.test(inputValue)) {
                    $ctrl.showError(this.getMessage('E104'));
                    return;
                }

                const intdigits = parseInt($ctrl.data('integerdigits'));
                const decdigits = parseInt($ctrl.data('decimaldigits'));
                const parts = inputValue.split('.');
                let intpart = parts[0];
                let decpart = parts.length > 1 ? parts[1] : "0";
                if (intpart.length > intdigits) {
                    $ctrl.showError(this.getMessage('E105'));
                    return;
                }

                if (decdigits > 0) {
                    if (decpart.length > decdigits) {
                        decpart = decpart.substr(0, decdigits);
                    }
                    else {
                        //decpart = ('0'.repeat(decdigits) + decpart).slice(-1 * decdigits);
                        decpart = (decpart + '0'.repeat(decdigits)).slice(0, decdigits);
                    }
                    inputValue = intpart + '.' + decpart;
                    $ctrl.val(inputValue);
                }
                else {
                    inputValue = intpart;
                    $ctrl.val(intpart);
                }
            }

            if (isMoney) {
                const regex = new RegExp(regexPattern.moeney);
                if (!regex.test(inputValue)) {
                    $ctrl.showError(this.getMessage('E104'));
                    return;
                }

                const digits = $ctrl.data('digits');
                inputValue = inputValue.replace(/,/g, '');
                if (inputValue.length > parseInt(digits)) {
                    $ctrl.showError(this.getMessage('E105'));
                    return;
                }
                $ctrl.val(Number(inputValue).toLocaleString());
            }
        }

        if (customValidation) {
            func = new Function('arg1', 'return ' + customValidation + '(arg1)')
            if (!func($ctrl)) {
                return false;
            }
        }

        if (type === 'radio')
            $('input[name="' + name + '"]:radio').hideError();
        else
            $ctrl.hideError();

        return true;
    },

    limitPager: function limitPager(count) {
        if ($('.pagination li').length > 7) {
            if ($('.pagination li.active').attr('data-page') <= 3) {
                $('.pagination li:gt(' + count + ')').hide();
                $('.pagination li:lt(' + count + ')').show();
                $('.pagination [data-page="next"]').show();
            } if ($('.pagination li.active').attr('data-page') > 3) {
                $('.pagination li:gt(0)').hide();
                $('.pagination [data-page="next"]').show();
                for (let i = (parseInt($('.pagination li.active').attr('data-page')) - 2); i <= (parseInt($('.pagination li.active').attr('data-page')) + 2); i++) {
                    $('.pagination [data-page="' + i + '"]').show();
                }
            }
        }
    },

    checkValidityOnSave: function checkValidityOnSave(selector) {
        let success = true;
        $(selector + ' :input:not(button):not(:hidden):not(:disabled):not([readonly])').each(function () {
            if (!common.checkValidityInput(this)) {
                success = false;
            }
        });
        return success;
    },
    
    bindValidationEvent: function bindValidationEvent(selector, exceptSelector) {
        var selector = selector + ' :input:not(button):not(:hidden):not(:disabled):not([readonly])';
        if (exceptSelector) {
            selector = selector + exceptSelector;
        }
        $(document).on('blur', selector, function (e) {
            return common.checkValidityInput(this);
        });
    },

    setValidationErrors: function setValidationErrors(errors)
    {
        for (key in errors) {
            const target = document.getElementById(key);
            $(target).showError(errors[key]);
        }
    },

    setFocusFirstError: function setFocusFirstError(form)
    {
        if (form) {
            const $target = $(form).getInvalidItems().get(0);
            if ($target) {
                $target.focus();
                return true;
            }
        }
        return false;
    },
     
    addPager: function addPager(tb,count) {
        
            $('.pagination [data-page="1"]').addClass('active');
            var totalRows = 0;
            totalRows = $(tb + " tbody tr").length;
        var maxRows = parseInt(count);
        if (totalRows > 10) {
            if (totalRows > maxRows) {
                var pagenum = Math.ceil(totalRows / maxRows);
                for (var i = 1; i <= pagenum;) {
                    $('.pagination #prev')
                        .before(
                            '<li data-page="' +
                            i +
                            '">\
								  <span>' +
                            i++ +
                            '<span class="sr-only">(current)</span></span>\
								</li>'
                        )
                        .show();
                }
            }
            var lastPage = 1;
            $('.pagination li').on('click', function (evt) {

                evt.stopImmediatePropagation();
                evt.preventDefault();
                var pageNum = $(this).attr('data-page');

                var maxRows = parseInt(count);

                if (pageNum == 'prev') {
                    if (lastPage == 1) {
                        return;
                    }
                    pageNum = --lastPage;
                }
                if (pageNum == 'next') {
                    if (lastPage == $('.pagination li').length - 2) {
                        return;
                    }
                    pageNum = ++lastPage;
                }

                lastPage = pageNum;
                var trIndex = 0;
                $('.pagination li').removeClass('active');
                $('.pagination [data-page="' + lastPage + '"]').addClass('active');

                common.limitPager(count);
                $(tb + ' tr:gt(0)').each(function () {
                    trIndex++;
                    if (
                        trIndex > maxRows * pageNum ||
                        trIndex <= maxRows * pageNum - maxRows
                    ) {
                        $(this).hide();
                    } else {
                        $(this).show();
                    }
                });
            });
            common.limitPager(count);
            $(tb + " tr:gt(" + count + ")").hide()}
           
        
           
      
    },
}
