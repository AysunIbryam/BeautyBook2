(function () {
    const form = document.getElementById("appointment-form");
    if (!form) return;

    const serviceSelect = document.getElementById("ServiceId");
    const employeeSelect = document.getElementById("EmployeeId");
    const dateInput = document.getElementById("Date");
    const timeSelect = document.getElementById("StartTime");

    const employeesUrl = form.dataset.employeesUrl;
    const slotsUrl = form.dataset.slotsUrl;

    const initialEmployeeId = employeeSelect.dataset.selected;
    const initialTime = timeSelect.dataset.selected;

    function fillSelect(select, items, placeholder) {
        select.replaceChildren();

        const first = document.createElement("option");
        first.value = "";
        first.textContent = placeholder;
        select.appendChild(first);

        items.forEach(function (item) {
            const option = document.createElement("option");
            option.value = item.value;
            option.textContent = item.text;
            select.appendChild(option);
        });
    }

    async function loadEmployees(preselectedId) {
        const serviceId = serviceSelect.value;

        if (!serviceId) {
            fillSelect(employeeSelect, [], "-- Първо избери услуга --");
            await loadSlots();
            return;
        }

        try {
            const response = await fetch(
                employeesUrl + "?serviceId=" + encodeURIComponent(serviceId));

            if (!response.ok) throw new Error();

            const employees = await response.json();

            fillSelect(
                employeeSelect,
                employees.map(function (e) { return { value: e.id, text: e.name }; }),
                "-- Избери служител --");

            if (preselectedId) {
                employeeSelect.value = preselectedId;
            }
        } catch (error) {
            fillSelect(employeeSelect, [], "Грешка при зареждане");
        }

        await loadSlots();
    }

    async function loadSlots(preselectedTime) {
        const serviceId = serviceSelect.value;
        const employeeId = employeeSelect.value;
        const date = dateInput.value;
        const currentValue = preselectedTime || timeSelect.value;

        fillSelect(timeSelect, [], "-- Избери час --");

        if (!serviceId || !employeeId || !date) return;

        timeSelect.disabled = true;

        try {
            const url = slotsUrl
                + "?date=" + encodeURIComponent(date)
                + "&serviceId=" + encodeURIComponent(serviceId)
                + "&employeeId=" + encodeURIComponent(employeeId);

            const response = await fetch(url);

            if (!response.ok) throw new Error();

            const slots = await response.json();

            if (slots.length === 0) {
                fillSelect(timeSelect, [], "Няма свободни часове");
            } else {
                fillSelect(
                    timeSelect,
                    slots.map(function (s) { return { value: s, text: s }; }),
                    "-- Избери час --");

                if (slots.includes(currentValue)) {
                    timeSelect.value = currentValue;
                }
            }
        } catch (error) {
            fillSelect(timeSelect, [], "Грешка при зареждане");
        } finally {
            timeSelect.disabled = false;
        }
    }

    serviceSelect.addEventListener("change", function () { loadEmployees(); });
    employeeSelect.addEventListener("change", function () { loadSlots(); });
    dateInput.addEventListener("change", function () { loadSlots(); });

    // Initial state (edit form or form re-rendered after a validation error).
    if (serviceSelect.value) {
        loadEmployees(initialEmployeeId).then(function () {
            return loadSlots(initialTime);
        });
    }
})();