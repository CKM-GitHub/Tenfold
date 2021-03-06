const sortTable = {
    getSortingTable: function (selector) {
        //sorting all header
        const table = document.getElementById(selector);
        const headers = table.querySelectorAll('th');
        const tableBody = table.querySelector('tbody');
        const rows = tableBody.querySelectorAll('tr');
        let old_colIndex = 0;

        const left_right_headers = table.querySelectorAll('th left_sort-by');

        // Track sort directions
        const directions = Array.from(headers).map(function (header) {
            return '';
        });

        // Transform the content of given cell in given column
        const transform = function (index, content) {
            // Get the data type of column           
            const type = headers[index].getAttribute('data-type');
            switch (type) {
                case 'number':
                    return parseFloat(content);
                case 'string':
                default:
                    return content;
            }
        };

        const sortColumn = function (index) {
            const rows = tableBody.querySelectorAll('tr');
            var col_index = [], multiplier, direction;
            if (headers[index].hasAttribute('ordercol-index'))
                col_index = headers[index].getAttribute('ordercol-index').split('_');
            else
                col_index[0] = index;
            // Get the current direction
            if (old_colIndex !== col_index[0]) {
                direction = 'asc';
                old_colIndex = col_index[0];
            }
            else
                direction = directions[col_index[0]] || 'asc';

            // A factor based on the direction
            if (index == 0 && (headers[0].innerText == 'NO.' || headers[0].innerText == 'No.')) {
                multiplier = 1;
                if (headers[index].hasAttribute('ordercol-direction'))
                    multiplier = headers[index].getAttribute('ordercol-direction') === 'asc' ? 1 : -1;
            }
            else
                multiplier = direction === 'asc' ? 1 : -1;

            const newRows = Array.from(rows);

            newRows.sort(function (rowA, rowB) {
                var count = col_index.length;
                let cellA, cellB, cellC, cellD, cellE, cellF, col_type1 = '', col_type2 = '', col_type3 = '';
                let a, b, c, d, e, f;
                var index1 = col_index[0];
                if (rowA.querySelectorAll('td')[index1].querySelectorAll('a').length > 0) {
                    cellA = rowA.querySelectorAll('td')[index1].querySelectorAll('a')[0].innerHTML;
                    cellB = rowB.querySelectorAll('td')[index1].querySelectorAll('a')[0].innerHTML;
                }
                else {
                    cellA = rowA.querySelectorAll('td')[index1].innerHTML;
                    cellB = rowB.querySelectorAll('td')[index1].innerHTML;
                }
                col_type1 = headers[index1].hasAttribute('data-type') ? headers[index1].getAttribute('data-type') : '';
                a = transform(index1, cellA);
                b = transform(index1, cellB);

                switch (count) {
                    case 1:
                        if (col_type1 == 'number') {
                            if (multiplier == 1) return a - b;
                            else return b - a;
                        }
                        else {
                            if (multiplier == 1) return a.localeCompare(b, "ja-JP");
                            else return b.localeCompare(a, "ja-JP");
                        }
                        break;

                    case 2: var index2 = col_index[1];
                        if (rowA.querySelectorAll('td')[index2].querySelectorAll('a').length > 0) {
                            cellC = rowA.querySelectorAll('td')[index2].querySelectorAll('a')[0].innerHTML;
                            cellD = rowB.querySelectorAll('td')[index2].querySelectorAll('a')[0].innerHTML;
                        }
                        else {
                            cellC = rowA.querySelectorAll('td')[index2].innerHTML;
                            cellD = rowB.querySelectorAll('td')[index2].innerHTML;
                        }
                        col_type2 = headers[index2].hasAttribute('data-type') ? headers[index2].getAttribute('data-type') : '';
                        c = transform(index2, cellC);
                        d = transform(index2, cellD);

                        if (a !== b) {
                            if (col_type1 == 'number') {
                                if (multiplier == 1) return a - b;
                                else return b - a;
                            }
                            else {
                                if (multiplier == 1) return a.localeCompare(b, "ja-JP");
                                else return b.localeCompare(a, "ja-JP");
                            }
                        }
                        else if (c !== d) {
                            if (col_type2 == 'number') {
                                return c - d;
                            }
                            else {
                                return c.localeCompare(d, "ja-JP");
                            }
                        }
                        break;

                    case 3: var index2 = col_index[1];
                        if (rowA.querySelectorAll('td')[index2].querySelectorAll('a').length > 0) {
                            cellC = rowA.querySelectorAll('td')[index2].querySelectorAll('a')[0].innerHTML;
                            cellD = rowB.querySelectorAll('td')[index2].querySelectorAll('a')[0].innerHTML;
                        }
                        else {
                            cellC = rowA.querySelectorAll('td')[index2].innerHTML;
                            cellD = rowB.querySelectorAll('td')[index2].innerHTML;
                        }
                        col_type2 = headers[index2].hasAttribute('data-type') ? headers[index2].getAttribute('data-type') : '';
                        c = transform(index2, cellC);
                        d = transform(index2, cellD);

                        var index3 = col_index[2];
                        if (rowA.querySelectorAll('td')[index3].querySelectorAll('a').length > 0) {
                            cellE = rowA.querySelectorAll('td')[index3].querySelectorAll('a')[0].innerHTML;
                            cellF = rowB.querySelectorAll('td')[index3].querySelectorAll('a')[0].innerHTML;
                        }
                        else {
                            cellE = rowA.querySelectorAll('td')[index3].innerHTML;
                            cellF = rowB.querySelectorAll('td')[index3].innerHTML;
                        }
                        col_type3 = headers[index3].hasAttribute('data-type') ? headers[index3].getAttribute('data-type') : '';
                        e = transform(index3, cellE);
                        f = transform(index3, cellF);

                        if (a !== b) {
                            if (col_type1 == 'number') {
                                if (multiplier == 1) return a - b;
                                else return b - a;
                            }
                            else {
                                if (multiplier == 1) return a.localeCompare(b, "ja-JP");
                                else return b.localeCompare(a, "ja-JP");
                            }
                        }
                        else if (c !== d) {
                            if (col_type2 == 'number') {
                                return c - d;
                            }
                            else {
                                return c.localeCompare(d, "ja-JP");
                            }
                        }
                        else if (e !== f) {
                            if (col_type3 == 'number') {
                                return e - f;
                            }
                            else {
                                return e.localeCompare(f, "ja-JP");
                            }
                        }
                        break;
                }
            });

            // Remove old rows
            [].forEach.call(rows, function (row) {
                tableBody.removeChild(row);
            });

            // Reverse the direction
            directions[col_index[0]] = direction === 'asc' ? 'desc' : 'asc';

            // Append new row 
            var count = 0;
            newRows.forEach(function (newRow) {
                if (headers[0].innerText == 'NO.' || headers[0].innerText == 'No.') {
                    count = count + 1;
                    newRow.querySelectorAll('td')[0].innerHTML = count;
                }
                tableBody.appendChild(newRow);
            });
        };

        const sortMultiColumn = function (ctrl, index) {
            const rows = tableBody.querySelectorAll('tr');
            var col_index = [], multiplier, col_ordertype;
            if ($(ctrl).attr('ordercol-index')) {
                col_ordertype = $(ctrl).attr('col_ordertype');
                col_index = $(ctrl).attr('ordercol-index').split('_');
            }
            else
                col_index[0] = index;

            // Get the current direction && A factor based on the direction
            if (!$(ctrl).attr('direction')) {
                $(ctrl).attr('direction', 'asc');
                multiplier = 1;
            }
            else {
                if (old_colIndex !== col_index[0]) {
                    multiplier = 1;
                    old_colIndex = col_index[0];
                }
                else
                    multiplier = $(ctrl).attr('direction') === 'asc' ? 1 : -1;
            }

            const newRows = Array.from(rows);

            newRows.sort(function (rowA, rowB) {
                var count = col_index.length;
                let cellA, cellB, cellC, cellD, cellE, cellF, col_type1 = '', col_type2 = '', col_type3 = '', PrefCD_A = '', PrefCD_B = '';
                let a, b, c, d, e, f;
                var index1 = col_index[0];
                if (rowA.querySelectorAll('td')[index1].querySelectorAll(col_ordertype).length > 0) {
                    cellA = rowA.querySelectorAll('td')[index1].querySelectorAll(col_ordertype)[0].innerHTML;
                    cellB = rowB.querySelectorAll('td')[index1].querySelectorAll(col_ordertype)[0].innerHTML;
                    if (col_ordertype == 'p' && rowA.querySelectorAll('td')[index1].querySelectorAll(col_ordertype).length > 1) {
                        PrefCD_A = rowA.querySelectorAll('td')[index1].querySelectorAll(col_ordertype)[1].innerHTML;
                        PrefCD_B = rowB.querySelectorAll('td')[index1].querySelectorAll(col_ordertype)[1].innerHTML;
                    }
                }
                else {
                    cellA = rowA.querySelectorAll('td')[index1].innerHTML;
                    cellB = rowB.querySelectorAll('td')[index1].innerHTML;
                }
                col_type1 = headers[index1].hasAttribute('data-type') ? headers[index1].getAttribute('data-type') : '';

                if (PrefCD_A !== '' || PrefCD_B !== '') {
                    cellA = PrefCD_A + cellA;
                    cellB = PrefCD_B + cellB;
                }
                a = transform(index1, cellA);
                b = transform(index1, cellB);

                switch (count) {
                    case 1:
                        if (col_type1 == 'number') {
                            if (multiplier == 1) return a - b;
                            else return b - a;
                        }
                        else {
                            if (multiplier == 1) return a.localeCompare(b, "ja-JP");
                            else return b.localeCompare(a, "ja-JP");
                        }
                        break;

                    case 2: var index2 = col_index[1];
                        if (rowA.querySelectorAll('td')[index2].querySelectorAll('a').length > 0) {
                            cellC = rowA.querySelectorAll('td')[index2].querySelectorAll('a')[0].innerHTML;
                            cellD = rowB.querySelectorAll('td')[index2].querySelectorAll('a')[0].innerHTML;
                        }
                        else {
                            cellC = rowA.querySelectorAll('td')[index2].innerHTML;
                            cellD = rowB.querySelectorAll('td')[index2].innerHTML;
                        }
                        col_type2 = headers[index2].hasAttribute('data-type') ? headers[index2].getAttribute('data-type') : '';
                        c = transform(index2, cellC);
                        d = transform(index2, cellD);

                        if (a !== b) {
                            if (col_type1 == 'number') {
                                if (multiplier == 1) return a - b;
                                else return b - a;
                            }
                            else {
                                if (multiplier == 1) return a.localeCompare(b, "ja-JP");
                                else return b.localeCompare(a, "ja-JP");
                            }
                        }
                        else if (c !== d) {
                            if (col_type2 == 'number') {
                                return c - d;
                            }
                            else {
                                return c.localeCompare(d, "ja-JP");
                            }
                        }
                        break;

                    case 3: var index2 = col_index[1];
                        if (rowA.querySelectorAll('td')[index2].querySelectorAll('a').length > 0) {
                            cellC = rowA.querySelectorAll('td')[index2].querySelectorAll('a')[0].innerHTML;
                            cellD = rowB.querySelectorAll('td')[index2].querySelectorAll('a')[0].innerHTML;
                        }
                        else {
                            cellC = rowA.querySelectorAll('td')[index2].innerHTML;
                            cellD = rowB.querySelectorAll('td')[index2].innerHTML;
                        }
                        col_type2 = headers[index2].hasAttribute('data-type') ? headers[index2].getAttribute('data-type') : '';
                        c = transform(index2, cellC);
                        d = transform(index2, cellD);

                        var index3 = col_index[2];
                        if (rowA.querySelectorAll('td')[index3].querySelectorAll('a').length > 0) {
                            cellE = rowA.querySelectorAll('td')[index3].querySelectorAll('a')[0].innerHTML;
                            cellF = rowB.querySelectorAll('td')[index3].querySelectorAll('a')[0].innerHTML;
                        }
                        else {
                            cellE = rowA.querySelectorAll('td')[index3].innerHTML;
                            cellF = rowB.querySelectorAll('td')[index3].innerHTML;
                        }
                        col_type3 = headers[index3].hasAttribute('data-type') ? headers[index3].getAttribute('data-type') : '';
                        e = transform(index3, cellE);
                        f = transform(index3, cellF);

                        if (a !== b) {
                            if (col_type1 == 'number') {
                                if (multiplier == 1) return a - b;
                                else return b - a;
                            }
                            else {
                                if (multiplier == 1) return a.localeCompare(b, "ja-JP");
                                else return b.localeCompare(a, "ja-JP");
                            }
                        }
                        else if (c !== d) {
                            if (col_type2 == 'number') {
                                return c - d;
                            }
                            else {
                                return c.localeCompare(d, "ja-JP");
                            }
                        }
                        else if (e !== f) {
                            if (col_type3 == 'number') {
                                return e - f;
                            }
                            else {
                                return e.localeCompare(f, "ja-JP");
                            }
                        }
                        break;
                }
            });

            // Remove old rows
            [].forEach.call(rows, function (row) {
                tableBody.removeChild(row);
            });

            // Reverse the direction
            $(ctrl).attr('direction', multiplier === 1 ? 'desc' : 'asc');

            // Append new row
            var count = 0;
            newRows.forEach(function (newRow) {
                count = count + 1;
                newRow.querySelectorAll('td')[0].innerHTML = count;
                tableBody.appendChild(newRow);
            });
        }

        const sortStatus = function (index) {

            // Get the current direction
            const direction = directions[index] || 'asc';

            const sortOredrs = { "One": 0, "Two": 1, "Three": 2, "Four": 3, "Five": 4, "Six": 5, "Seven": 6, "Eight": 7, "Nine": 8 };

            const sortedRows = [...rows].sort((a, b) =>
                sortOredrs[a.children[1].className] - sortOredrs[b.children[1].className]
            );

            const sortBackOredrs = { "Nine": 0, "Eight": 1, "Seven": 2, "Six": 3, "Five": 4, "Four": 5, "Three": 6, "Two": 7, "One": 8 };
            const sortedBackRows = [...rows].sort((a, b) =>
                sortBackOredrs[a.children[1].className] - sortBackOredrs[b.children[1].className]
            );

            // Remove old rows
            [].forEach.call(rows, function (row) {
                tableBody.removeChild(row);
            });

            // Reverse the direction
            directions[index] = direction === 'asc' ? 'desc' : 'asc';
            if (direction === 'asc') {
                // Append new row
                sortedRows.forEach(function (newRow) {
                    tableBody.appendChild(newRow);
                });
            }
            else {
                // Append new row
                sortedBackRows.forEach(function (newRow) {
                    tableBody.appendChild(newRow);
                });
            }
        };

        const sortMix_td_aTag = function (index) {

            // Get the current direction
            const direction = directions[index] || 'asc';

            // A factor based on the direction
            const multiplier = direction === 'asc' ? 1 : -1;

            const newRows = Array.from(rows);

            newRows.sort(function (rowA, rowB) {
                const cellA = rowA.querySelectorAll('td')[index].firstChild.nextSibling.innerHTML;
                const cellB = rowB.querySelectorAll('td')[index].firstChild.nextSibling.innerHTML;                

                const a = transform(index, cellA);
                const b = transform(index, cellB);

                switch (true) {
                    case a > b:
                        return 1 * multiplier;
                    case a < b:
                        return -1 * multiplier;
                    case a === b:
                        return 0;
                }
            });

            // Remove old rows
            [].forEach.call(rows, function (row) {
                tableBody.removeChild(row);
            });

            // Reverse the direction
            directions[index] = direction === 'asc' ? 'desc' : 'asc';

            // Append new row
            newRows.forEach(function (newRow) {
                tableBody.appendChild(newRow);
            });
        };

        const sortStatusTSellerList = function (index) {

            // Get the current direction
            const direction = directions[index] || 'asc';

            const sortOredrs = { "t_seller_list_one": 0, "t_seller_list_two": 1, "t_seller_list_three": 2};

            const sortedRows = [...rows].sort((a, b) =>
                sortOredrs[a.children[1].className] - sortOredrs[b.children[1].className]
            );

            const sortBackOredrs = { "t_seller_list_three": 0, "t_seller_list_two": 1, "t_seller_list_one": 2};
            const sortedBackRows = [...rows].sort((a, b) =>
                sortBackOredrs[a.children[1].className] - sortBackOredrs[b.children[1].className]
            );

            // Remove old rows
            [].forEach.call(rows, function (row) {
                tableBody.removeChild(row);
            });

            // Reverse the direction
            directions[index] = direction === 'asc' ? 'desc' : 'asc';
            if (direction === 'asc') {
                // Append new row
                sortedRows.forEach(function (newRow) {
                    tableBody.appendChild(newRow);
                });
            }
            else {
                // Append new row
                sortedBackRows.forEach(function (newRow) {
                    tableBody.appendChild(newRow);
                });
            }
        };
        //
        const sortStatusTAssessment = function (index) {

            // Get the current direction
            const direction = directions[index] || 'asc';

            const sortOredrs = { "One": 0, "Two": 1, "Three": 2, "Four": 3, "Five": 4, "Six": 5, "Seven": 6, "Eight": 7, "Nine": 8, "Ten": 9 };

            const sortedRows = [...rows].sort((a, b) =>
                sortOredrs[a.children[1].className] - sortOredrs[b.children[1].className]
            );

            const sortBackOredrs = { "Ten": 0,"Nine": 1, "Eight": 2, "Seven": 3, "Six": 4, "Five": 5, "Four": 6, "Three": 7, "Two": 8, "One": 9 };
            const sortedBackRows = [...rows].sort((a, b) =>
                sortBackOredrs[a.children[1].className] - sortBackOredrs[b.children[1].className]
            );

            // Remove old rows
            [].forEach.call(rows, function (row) {
                tableBody.removeChild(row);
            });

            // Reverse the direction
            directions[index] = direction === 'asc' ? 'desc' : 'asc';
            if (direction === 'asc') {
                // Append new row
                sortedRows.forEach(function (newRow) {
                    tableBody.appendChild(newRow);
                });
            }
            else {
                // Append new row
                sortedBackRows.forEach(function (newRow) {
                    tableBody.appendChild(newRow);
                });
            }
        };

        [].forEach.call(headers, function (header, index) {
            header.addEventListener('click', function () {
                if ($(this).hasClass('sort-by')) {
                    sortColumn(index);
                }

                if ($(this).hasClass('sort-mix-td-and-a')) {
                    sortMix_td_aTag(index);
                }
                
                if ($(this).hasClass('sort-status')) {
                    sortStatus(index);
                }
                if ($(this).hasClass('sort-status-t-seller-list')) {
                    sortStatusTSellerList(index);
                }
                if ($(this).hasClass('sort-status-t-seller-assessment')) {
                    sortStatusTAssessment(index);
                }
            });

            if (header.querySelectorAll('span').length > 0) {
                [].forEach.call(header.querySelectorAll('span'), function (span, index) {
                    if ($(span).hasClass('sort-multi')) {
                        span.addEventListener('click', function () {
                            if ($(this).hasClass('sort-multi')) {
                                sortMultiColumn(this, index);
                            }
                        })
                    }
                })
            }
        });
    }   
}
