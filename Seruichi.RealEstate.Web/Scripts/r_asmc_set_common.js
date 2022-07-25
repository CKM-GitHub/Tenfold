function get_optionName(optionkbn) {
    if (optionkbn === 1) return '総戸数';
    if (optionkbn === 2) return '所在階';
    if (optionkbn === 3) return '所在階';
    if (optionkbn === 4) return '専有面積';
    if (optionkbn === 5) return 'バルコニー';
    if (optionkbn === 6) return '主採光';
    if (optionkbn === 8) return '部屋数';
    if (optionkbn === 9) return 'バス・トイレ';
    if (optionkbn === 10) return '土地権利';
    if (optionkbn === 11) return '賃貸状況';
    if (optionkbn === 12) return '管理状況';
    if (optionkbn === 13) return '売却希望時期';
    return '';
}

function get_optDetail(optionkbn, categorykbn, value1) {

    if (optionkbn === 2) return '1階';
    else if (optionkbn === 3) return '最上階';
    else if (optionkbn === 5) return 'バルコニーなし';
    else if (optionkbn === 6) return '北向き';
    else if (optionkbn === 7) return '角部屋';
    else if (optionkbn === 9) {
        if (categorykbn === 1) return 'セパレート';
        if (categorykbn === 2) return 'ユニットバス';
        if (categorykbn === 3) return 'シャワーブース';
    }
    else if (optionkbn === 10) return '借地権';
    else if (optionkbn === 11) {
        if (categorykbn === 1) return '賃貸中';
        if (categorykbn === 2) return 'サブリース';
        if (categorykbn === 3) return '空室';
    }
    else if (optionkbn === 12) {
        if (categorykbn === 1) return '自主管理';
        if (categorykbn === 2) return '管理委託';
    }
    else if (optionkbn === 13) {
        if (categorykbn === 1) return '2週間以上';
        if (categorykbn === 2) return '1ヶ月以上';
        if (categorykbn === 3) return '3ヶ月以上';
        if (categorykbn === 4) return '6ヶ月以上';
        if (categorykbn === 5) return '1年以上';
        if (categorykbn === 6) return 'その他';
    }

    if (value1) {
        return String(value1);
    }

    return '';
}

function get_handlingKBN1Text(handlingKBN) {
    if (handlingKBN === 1) return '以内';
    if (handlingKBN === 4) return '～';
    return '';
}

function get_notApplicableFLGText() {
    return '査定・買取対象外';
}

function get_trHtml(rowData) {
    if (rowData.OptionKBN === 0) return '';

    let td1 = '', td2 = '', td3 = '', td4 = '';

    //条件名
    td1 = '<td><a class="text-danger js-opt-xbtn"><i class="fa fa-close pe-1"></i></a>' + get_optionName(rowData.OptionKBN) + '</td>'

    //条件内容
    if (rowData.OptionKBN === 1) {
        td2 = '<td>総戸数 <span class="text-danger">' + rowData.Value1 + '</span>戸 以内</td>'
    }
    else if (rowData.OptionKBN === 4) {
        td2 = '<td><span class="text-danger">' + rowData.Value1 + '</span>㎡'
            + '<span class="text-danger" >' + get_handlingKBN1Text(rowData.HandlingKBN1) + '</span ></td > '
    }
    else if (rowData.OptionKBN === 8) {
        td2 = '<td><span class="text-danger">' + rowData.Value1 + '</span>部屋</td>';
    }
    else if (rowData.OptionKBN === 9 || rowData.OptionKBN === 11 || rowData.OptionKBN === 12 || rowData.OptionKBN === 13) {
        td2 = '<td><span class="text-danger">' + get_optDetail(rowData.OptionKBN, rowData.CategoryKBN) + '</span></td>';
    }
    else {
        td2 = '<td><span class="text-danger">' + get_optDetail(rowData.OptionKBN) + '</span></td>';
    }

    //買取利回り率
    if (rowData.OptionKBN === 3) {
        td3 = '<td><span class="text-danger">' + rowData.IncDecRate + '</span>%</td>'
    }
    else {
        if (rowData.NotApplicableFLG === 0) {
            td3 = '<td><span class="text-danger">' + rowData.IncDecRate + '</span>%</td>'
        } else {
            td3 = '<td>' + get_notApplicableFLGText() + '</td>';
        }
    }

    //更新用データ
    td4 = '<td hidden>' + JSON.stringify(rowData) + '</td>'

    return '<tr>' + td1 + td2 + td3 + td4 + '</tr>';
}

function sort_optTable(tableSelector) {
    let table = document.querySelector(tableSelector);
    let sortArray = new Array;

    for (let r = 1; r < table.rows.length; r++) {
        const rowData = JSON.parse(table.rows[r].cells[3].textContent);
        let obj = new Object;
        obj.row = table.rows[r];
        obj.value = rowData.OptionKBN * 100 + r;
        sortArray.push(obj);
    }

    sortArray.sort(compareNumber);

    let tbody = document.querySelector(tableSelector + ' tbody');
    $(tbody).empty();
    for (let i = 0; i < sortArray.length; i++) {
        tbody.appendChild(sortArray[i].row);
    }
}

function compareNumber(a, b) {
    return a.value - b.value;
}

