const sortTable = {
    getSortingTable: function (selector) {
        //sorting all header
        const table = document.getElementById(selector);
        const headers = table.querySelectorAll('th');
        const tableBody = table.querySelector('tbody');
        const rows = tableBody.querySelectorAll('tr');

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
            var col_index;
            if (headers[index].getAttribute('ordercol-index').length > 0)
                col_index = headers[index].getAttribute('ordercol-index').split('_');
            else
                col_index = index;
            // Get the current direction
            const direction = directions[col_index[0]] || 'asc';

            // A factor based on the direction
            const multiplier = direction === 'asc' ? 1 : -1;

            const newRows = Array.from(rows);
           
            newRows.sort(function (rowA, rowB) {
                var count = col_index.length;
                let cellA, cellB, cellC, cellD, cellE, cellF;
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
                a = transform(index1, cellA);
                b = transform(index1, cellB);

                switch (count) {
                    case 1: if (multiplier == 1) return a.localeCompare(b, "ja-JP");
                        else return b.localeCompare(a, "ja-JP");

                    case 2: var index2 = col_index[1];
                        if (rowA.querySelectorAll('td')[index2].querySelectorAll('a').length > 0) {
                            cellC = rowA.querySelectorAll('td')[index2].querySelectorAll('a')[0].innerHTML;
                            cellD = rowB.querySelectorAll('td')[index2].querySelectorAll('a')[0].innerHTML;
                        }
                        else {
                            cellC = rowA.querySelectorAll('td')[index2].innerHTML;
                            cellD = rowB.querySelectorAll('td')[index2].innerHTML;
                        }
                        c = transform(index2, cellC);
                        d = transform(index2, cellD);

                        if (a !== b) {
                            if (multiplier == 1) return a.localeCompare(b, "ja-JP");
                            else return b.localeCompare(a, "ja-JP");
                        }
                        else if (c !== d) {
                            if (multiplier == 1) return c.localeCompare(d, "ja-JP");
                            else return d.localeCompare(c, "ja-JP");
                        }

                    case 3: var index2 = col_index[1];
                        if (rowA.querySelectorAll('td')[index2].querySelectorAll('a').length > 0) {
                            cellC = rowA.querySelectorAll('td')[index2].querySelectorAll('a')[0].innerHTML;
                            cellD = rowB.querySelectorAll('td')[index2].querySelectorAll('a')[0].innerHTML;
                        }
                        else {
                            cellC = rowA.querySelectorAll('td')[index2].innerHTML;
                            cellD = rowB.querySelectorAll('td')[index2].innerHTML;
                        }
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
                        e = transform(index3, cellE);
                        f = transform(index3, cellF);

                        if (a !== b) {
                            if (multiplier == 1) return a.localeCompare(b, "ja-JP");
                            else return b.localeCompare(a, "ja-JP");
                        }
                        else if (c !== d) {
                            if (multiplier == 1) return c.localeCompare(d, "ja-JP");
                            else return d.localeCompare(c, "ja-JP");
                        }
                        else if (e !== f) {
                            if (multiplier == 1) return e.localeCompare(f, "ja-JP");
                            else return f.localeCompare(e, "ja-JP");
                        }
                }
            });

            // Remove old rows
            [].forEach.call(rows, function (row) {
                tableBody.removeChild(row);
            });

            // Reverse the direction
            directions[col_index[0]] = direction === 'asc' ? 'desc' : 'asc';

            // Append new row
            newRows.forEach(function (newRow) {
                tableBody.appendChild(newRow);
            });
        };

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
        });
    }   
}
