const sortTable = {
    getSortingTable: function (selector) {
        //sorting all header
        const table = document.getElementById(selector);
        const headers = table.querySelectorAll('th');
        const tableBody = table.querySelector('tbody');
        const rows = tableBody.querySelectorAll('tr');

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

            // Get the current direction
            const direction = directions[index] || 'asc';

            // A factor based on the direction
            const multiplier = direction === 'asc' ? 1 : -1;

            const newRows = Array.from(rows);
           
            newRows.sort(function (rowA, rowB) {
                const cellA = rowA.querySelectorAll('td')[index].innerHTML;
                const cellB = rowB.querySelectorAll('td')[index].innerHTML;               

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
        //
        [].forEach.call(headers, function (header, index) {
            header.addEventListener('click', function () {
                if ($(this).hasClass('sort-by')) {
                    sortColumn(index);
                }
                if ($(this).hasClass('sort-status')) {
                    sortStatus(index);
                }                
            });
        });
    }

   
}
