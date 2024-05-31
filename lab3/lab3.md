# Lab 3: Relational Algebra

## Part 1 - Joins

### Query 1: \( T1 \bowtie_{T1.A = T2.A} T2 \)

This query performs an equi-join on T1 and T2, where T1's column A equals T2's column A.

Resulting schema:
| T1.A | Q | R | T2.A | B | C |
|------|---|---|------|---|---|

Resulting table:
| T1.A | Q | R | T2.A | B | C |
|------|---|---|------|---|---|
| 20   | a | 5 | 20   | b | 6 |
| 20   | a | 5 | 20   | b | 5 |

### Query 2: \( T1 \bowtie_{T1.Q = T2.B} T2 \)

This query performs an equi-join on T1 and T2, where T1's column Q equals T2's column B.

Resulting schema:
| A  | T1.Q | R | T2.A | B | C |
|----|------|---|------|---|---|

Resulting table:
| A  | T1.Q | R | T2.A | B | C |
|----|------|---|------|---|---|
| 25 | b    | 8 | 20   | b | 6 |
| 25 | b    | 8 | 20   | b | 5 |

### Query 3: \( T1 \bowtie T2 \)

This query is the Cartesian product of T1 and T2, with no join condition.

Resulting schema:
| T1.A | Q | R | T2.A | B | C |
|------|---|---|------|---|---|

Resulting table:
| T1.A | Q | R | T2.A | B | C |
|------|---|---|------|---|---|
| 20   | a | 5 | 20   | b | 6 |
| 20   | a | 5 | 45   | c | 3 |
| 20   | a | 5 | 20   | b | 5 |
| 25   | b | 8 | 20   | b | 6 |
| 25   | b | 8 | 45   | c | 3 |
| 25   | b | 8 | 20   | b | 5 |
| 35   | a | 6 | 20   | b | 6 |
| 35   | a | 6 | 45   | c | 3 |
| 35   | a | 6 | 20   | b | 5 |

### Query 4: \( T1 \bowtie_{T1.A = T2.A \land T1.R = T2.C} T2 \)

This query performs a multi-condition equi-join on T1 and T2, where T1's column A equals T2's column A and T1's column R equals T2's column C.

Resulting schema:
| T1.A | Q | R | T2.A | B | C |
|------|---|---|------|---|---|

Resulting table:
| T1.A | Q | R | T2.A | B | C |
|------|---|---|------|---|---|
| 20   | a | 5 | 20   | b | 5 |

## Part 2 - Chess Queries

1. **Find the names of any player with an Elo rating of 2850 or higher.**
$\Pi_{\text{Name}}(\sigma_{\text{Elo} \geq 2850}(\text{Players}))$

2. **Find the names of any player who has ever played a game as white.**
$\Pi_{\text{Name}}(\text{Players} \bowtie \rho_{(\text{pID} \rightarrow \text{wpID})}(\text{Games}))$

3. **Find the names of any player who has ever won a game as white.**
$\Pi_{\text{Name}}(\sigma_{\text{Result} = '1-0'}(\text{Players} \bowtie \rho_{(\text{pID} \rightarrow \text{wpID})}(\text{Games})))$

4. **Find the names of any player who played any games in 2018.**
$\Pi_{\text{Name}}((\text{Players} \bowtie \rho_{(\text{pID} \rightarrow \text{wpID})}(\sigma_{\text{eID} = 2 \vee \text{eID} = 3}(\text{Games}))) \cup (\text{Players} \bowtie \rho_{(\text{pID} \rightarrow \text{bpID})}(\sigma_{\text{eID} = 2 \vee \text{eID} = 3}(\text{Games}))))$

5. **Find the names and dates of any event in which Magnus Carlsen lost a game.**
$\Pi_{\text{Name}, \text{Year}}(\sigma_{(\text{wpID} = 1 \wedge \text{Result} = '0-1') \vee (\text{bpID} = 1 \wedge \text{Result} = '1-0')}(\text{Events} \bowtie \rho_{(\text{eID} \rightarrow \text{eID1})}(\text{Games} \bowtie \rho_{(\text{wpID} \rightarrow \text{pID}, \text{bpID} \rightarrow \text{pID1})}(\rho_{(\text{Name} \rightarrow \text{Name1}, \text{pID} \rightarrow \text{pID1})}(\sigma_{\text{Name} = '\text{Magnus Carlsen}'}(\text{Players}))))))$

6. **Find the names of all opponents of Magnus Carlsen. An opponent is someone who he has played a game against.**
$(\Pi_{\text{Name1}}(\rho_{(\text{Name} \rightarrow \text{Name1}, \text{pID} \rightarrow \text{pID1})}(\text{Players}) \bowtie \rho_{(\text{pID} \rightarrow \text{bpID})}(\sigma_{\text{wpID} = 1}(\text{Games})))) \cup (\Pi_{\text{Name1}}(\rho_{(\text{Name} \rightarrow \text{Name1}, \text{pID} \rightarrow \text{pID1})}(\text{Players}) \bowtie \rho_{(\text{pID} \rightarrow \text{wpID})}(\sigma_{\text{bpID} = 1}(\text{Games}))))$

## Part 3 - LMS Queries

### Part 3.1

Table: 

| Name ( varchar(255) )    |
|----------|
| Hermione |
| Harry    |

Find the names of students who have never received a "C" grade.

### Part 3.2

| Name ( varchar(255) )     |
|----------|
| Hermione |

Find the names of all students born in the same year as Ron, excluding Ron.

### Part 3.3

Empty set (no results)

Find the names of the courses in which all students are enrolled.

### Part 4

$\text{CoursesStartWithThree} \gets \Pi_{cID}(\sigma_{cID \geq 3000 \land cID < 4000} (\text{Courses}))$

$\text{StudentsIDs} \gets \Pi_{sID} (\text{CoursesStartWithThree} / \text{Enrolled})$

$\Pi_{Name} (\text{Students} \bowtie \text{StudentsIDs})$


