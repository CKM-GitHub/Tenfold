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
                    return parseFloat(content.replace(/,/gi, ''));
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
            if (index == 0) {
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
                count = count + 1;
                newRow.querySelectorAll('td')[0].innerHTML = count;
                tableBody.appendChild(newRow);
            });
        };

        [].forEach.call(headers, function (header, index) {
            header.addEventListener('click', function () {
                if ($(this).hasClass('sort-by')) {
                    sortColumn(index);
                }
            });
        });
    }   
}
