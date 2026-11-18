"""Generates the EF6 Database-First model (EDMX + entity classes + context) from one English spec.

Usage: python3 gen_model.py <LawyerProject/DB dir>
"""
import os, sys, uuid

NS = 'QuedinModel'
CTX = 'QuedinDbContext'
OUT = sys.argv[1]

# column: (name, sql type, length/precision, nullable, flags)  flags: 'id' identity, 'rv' rowversion
E = [
    ('User', 'Users', [
        ('UserId', 'int', None, False, 'id'), ('UserName', 'nvarchar', 50, False, ''),
        ('PasswordHash', 'nvarchar', 200, False, ''), ('Role', 'nvarchar', 50, False, ''),
        ('IsActive', 'bit', None, False, ''), ('FailedLogins', 'int', None, False, ''),
        ('LockedUntil', 'datetime', None, True, '')]),
    ('AuditEntry', 'AuditLog', [
        ('AuditEntryId', 'int', None, False, 'id'), ('OccurredAt', 'datetime', None, False, ''),
        ('UserName', 'nvarchar', 50, True, ''), ('Action', 'nvarchar', 50, False, ''),
        ('Entity', 'nvarchar', 50, True, ''), ('EntityId', 'nvarchar', 50, True, ''),
        ('Details', 'nvarchar', 400, True, '')]),
    ('Setting', 'Settings', [
        ('SettingKey', 'nvarchar', 50, False, ''), ('Value', 'nvarchar', 200, True, '')]),
    ('Holiday', 'Holidays', [
        ('HolidayDate', 'date', None, False, ''), ('Name', 'nvarchar', 50, False, '')]),
    ('Customer', 'Customers', [
        ('CustomerId', 'nvarchar', 20, False, ''), ('FirstName', 'nvarchar', 50, False, ''),
        ('LastName', 'nvarchar', 25, False, ''), ('City', 'nvarchar', 50, True, ''),
        ('Street', 'nvarchar', 50, True, ''), ('HouseNumber', 'nvarchar', 5, True, ''),
        ('PostalCode', 'nvarchar', 7, True, ''), ('Phone', 'nvarchar', 20, True, ''),
        ('MobilePhone', 'nvarchar', 20, True, ''), ('Email', 'nvarchar', 50, True, ''),
        ('BirthDate', 'date', None, True, ''), ('PersonType', 'nvarchar', 50, False, ''),
        ('IdType', 'nvarchar', 20, True, ''), ('RowVersion', 'rowversion', None, False, 'rv')]),
    ('Court', 'Courts', [
        ('CourtId', 'int', None, False, 'id'), ('Name', 'nvarchar', 50, False, '')]),
    ('CaseType', 'CaseTypes', [
        ('CaseTypeId', 'int', None, False, 'id'), ('Name', 'nvarchar', 50, False, ''),
        ('BaseFee', 'money', None, True, ''), ('CourtFee', 'money', None, True, '')]),
    ('Procedure', 'Procedures', [
        ('ProcedureId', 'int', None, False, 'id'), ('Name', 'nvarchar', 50, False, ''),
        ('DurationDays', 'int', None, False, ''), ('Category', 'nvarchar', 50, False, '')]),
    ('CourtCaseType', 'CourtCaseTypes', [
        ('CourtCaseTypeId', 'int', None, False, 'id'), ('CourtId', 'int', None, False, ''),
        ('CaseTypeId', 'int', None, False, '')]),
    ('CaseTypeStep', 'CaseTypeSteps', [
        ('CaseTypeStepId', 'int', None, False, 'id'), ('CaseTypeId', 'int', None, False, ''),
        ('ProcedureId', 'int', None, False, ''), ('StepOrder', 'int', None, False, '')]),
    ('LegalCase', 'Cases', [
        ('CaseId', 'int', None, False, 'id'), ('ClientId', 'nvarchar', 20, False, ''),
        ('CaseTypeId', 'int', None, False, ''), ('IsInCourt', 'bit', None, True, ''),
        ('CourtId', 'int', None, True, ''), ('CurrentProcedureId', 'int', None, False, ''),
        ('PowerOfAttorney', 'nvarchar', 50, False, ''), ('IsPaid', 'bit', None, False, ''),
        ('Advance', 'money', None, False, ''), ('IsAdvancePaid', 'bit', None, True, ''),
        ('Block', 'nvarchar', 50, True, ''), ('Parcel', 'nvarchar', 50, True, ''),
        ('SubParcel', 'nvarchar', 50, True, ''), ('PropertyAddress', 'nvarchar', 50, True, ''),
        ('ClaimAmount', 'money', None, True, ''), ('AwardedAmount', 'money', None, True, ''),
        ('FeePercent', 'float', None, True, ''), ('OpposingLawyerId', 'nvarchar', 20, False, ''),
        ('OpposingPartyId', 'nvarchar', 20, False, ''),
        # the case type's fees when the case was opened, so later catalogue changes do not alter old totals
        ('MinimumFee', 'money', None, True, ''), ('CourtFee', 'money', None, True, ''), ('RowVersion', 'rowversion', None, False, 'rv')]),
    ('CaseStep', 'CaseSteps', [
        ('CaseStepId', 'int', None, False, 'id'), ('CaseId', 'int', None, False, ''),
        ('ProcedureId', 'int', None, False, ''), ('DueDate', 'date', None, True, ''),
        ('DoneDate', 'date', None, True, ''), ('IsDone', 'bit', None, True, ''),
        ('Notes', 'nvarchar', 200, True, ''), ('DurationDays', 'int', None, True, ''),
        ('IsFixedDate', 'bit', None, False, '')]),
    ('Payment', 'Payments', [
        ('PaymentId', 'int', None, False, 'id'), ('CaseId', 'int', None, False, ''),
        ('BalanceAfter', 'money', None, False, ''), ('Method', 'nvarchar', 50, False, ''),
        ('CardLast4', 'nvarchar', 8, True, ''), ('ChequeNumber', 'nchar', 10, True, ''),
        ('BankNumber', 'nchar', 3, True, ''), ('BranchNumber', 'nchar', 3, True, ''),
        ('AccountNumber', 'nvarchar', 50, True, ''), ('Amount', 'money', None, False, ''),
        ('PaidOn', 'date', None, False, ''), ('Description', 'nvarchar', 50, True, ''),
        ('Reference', 'nvarchar', 50, True, ''), ('ChequeDate', 'date', None, True, ''),
        ('Kind', 'nvarchar', 20, True, ''), ('VatRate', 'decimal', (5, 4), True, ''),
        ('IsCancelled', 'bit', None, False, ''), ('CancelsPaymentId', 'int', None, True, '')]),
]
KEYS = {'Setting': 'SettingKey', 'Holiday': 'HolidayDate', 'Customer': 'CustomerId'}

# (principal entity, principal key, dependent entity, fk, collection nav on principal, reference nav on dependent)
A = [
    ('Court', 'CourtId', 'CourtCaseType', 'CourtId', 'CourtCaseTypes', 'Court'),
    ('CaseType', 'CaseTypeId', 'CourtCaseType', 'CaseTypeId', 'CourtCaseTypes', 'CaseType'),
    ('CaseType', 'CaseTypeId', 'CaseTypeStep', 'CaseTypeId', 'Steps', 'CaseType'),
    ('Procedure', 'ProcedureId', 'CaseTypeStep', 'ProcedureId', 'CaseTypeSteps', 'Procedure'),
    ('LegalCase', 'CaseId', 'CaseStep', 'CaseId', 'Steps', 'Case'),
    ('Procedure', 'ProcedureId', 'CaseStep', 'ProcedureId', 'CaseSteps', 'Procedure'),
    ('LegalCase', 'CaseId', 'Payment', 'CaseId', 'Payments', 'Case'),
    ('CaseType', 'CaseTypeId', 'LegalCase', 'CaseTypeId', 'Cases', 'CaseType'),
    ('Court', 'CourtId', 'LegalCase', 'CourtId', 'Cases', 'Court'),
    ('Procedure', 'ProcedureId', 'LegalCase', 'CurrentProcedureId', 'CurrentCases', 'CurrentProcedure'),
    ('Customer', 'CustomerId', 'LegalCase', 'ClientId', 'Cases', 'Client'),
]

ent = {e[0]: e for e in E}
table = {e[0]: e[1] for e in E}


def key(e):
    return KEYS.get(e, ent[e][2][0][0])


def col(e, name):
    return next(c for c in ent[e][2] if c[0] == name)


def assoc_name(a):
    return 'FK_%s_%s' % (table[a[2]], table[a[0]]) + ('_Current' if a[3] == 'CurrentProcedureId' else '')


# ---------------------------------------------------------------- SSDL
def ssdl_prop(c):
    n, t, l, null, f = c
    a = 'Name="%s" Type="%s"' % (n, t)
    if t in ('nvarchar', 'nchar'):
        a += ' MaxLength="%d"' % l
    if t == 'decimal':
        a += ' Precision="%d" Scale="%d"' % l
    if f == 'id':
        a += ' StoreGeneratedPattern="Identity"'
    if f == 'rv':
        a += ' StoreGeneratedPattern="Computed"'
    if not null:
        a += ' Nullable="false"'
    return '          <Property %s />' % a


def ssdl():
    o = ['    <Schema Namespace="%s.Store" Provider="System.Data.SqlClient" ProviderManifestToken="2012" Alias="Self" '
         'xmlns:store="http://schemas.microsoft.com/ado/2007/12/edm/EntityStoreSchemaGenerator" '
         'xmlns:customannotation="http://schemas.microsoft.com/ado/2013/11/edm/customannotation" '
         'xmlns="http://schemas.microsoft.com/ado/2009/11/edm/ssdl">' % NS]
    for e, t, cols in E:
        o.append('        <EntityType Name="%s">' % t)
        o.append('          <Key>\n            <PropertyRef Name="%s" />\n          </Key>' % key(e))
        o += [ssdl_prop(c) for c in cols]
        o.append('        </EntityType>')
    for a in A:
        n = assoc_name(a)
        pm = '0..1' if col(a[2], a[3])[3] else '1'
        o.append('        <Association Name="%s">' % n)
        o.append('          <End Role="%s" Type="Self.%s" Multiplicity="%s" />' % (table[a[0]], table[a[0]], pm))
        o.append('          <End Role="%s" Type="Self.%s" Multiplicity="*" />' % (table[a[2]], table[a[2]]))
        o.append('          <ReferentialConstraint>\n            <Principal Role="%s">\n              <PropertyRef Name="%s" />\n'
                 '            </Principal>\n            <Dependent Role="%s">\n              <PropertyRef Name="%s" />\n'
                 '            </Dependent>\n          </ReferentialConstraint>' % (table[a[0]], a[1], table[a[2]], a[3]))
        o.append('        </Association>')
    o.append('        <EntityContainer Name="%sStoreContainer">' % NS)
    for e, t, cols in E:
        o.append('          <EntitySet Name="%s" EntityType="Self.%s" Schema="dbo" store:Type="Tables" />' % (t, t))
    for a in A:
        n = assoc_name(a)
        o.append('          <AssociationSet Name="%s" Association="Self.%s">\n            <End Role="%s" EntitySet="%s" />\n'
                 '            <End Role="%s" EntitySet="%s" />\n          </AssociationSet>' % (n, n, table[a[0]], table[a[0]], table[a[2]], table[a[2]]))
    o.append('        </EntityContainer>\n      </Schema>')
    return '\n'.join(o)


# ---------------------------------------------------------------- CSDL
EDM = {'int': 'Int32', 'nvarchar': 'String', 'nchar': 'String', 'money': 'Decimal', 'decimal': 'Decimal', 'bit': 'Boolean',
       'date': 'DateTime', 'datetime': 'DateTime', 'float': 'Double', 'rowversion': 'Binary'}


def csdl_prop(c):
    n, t, l, null, f = c
    a = 'Name="%s" Type="%s"' % (n, EDM[t])
    if t == 'nvarchar':
        a += ' MaxLength="%d" FixedLength="false" Unicode="true"' % l
    if t == 'nchar':
        a += ' MaxLength="%d" FixedLength="true" Unicode="true"' % l
    if t == 'money':
        a += ' Precision="19" Scale="4"'
    if t == 'decimal':
        a += ' Precision="%d" Scale="%d"' % l
    if t == 'date':
        a += ' Precision="0"'
    if t == 'datetime':
        a += ' Precision="3"'
    if t == 'rowversion':
        a += ' MaxLength="8" FixedLength="true"'
    a += ' Nullable="%s"' % ('true' if null else 'false')
    if f == 'id':
        a += ' annotation:StoreGeneratedPattern="Identity"'
    if f == 'rv':
        a += ' annotation:StoreGeneratedPattern="Computed" ConcurrencyMode="Fixed"'
    return '          <Property %s />' % a


def navs(e):
    out = []
    for a in A:
        n = assoc_name(a)
        if a[0] == e:
            out.append((a[4], n, a[0], a[2], True))
        if a[2] == e:
            out.append((a[5], n, a[2], a[0], False))
    return out


def csdl():
    o = ['    <Schema Namespace="%s" Alias="Self" annotation:UseStrongSpatialTypes="false" '
         'xmlns:annotation="http://schemas.microsoft.com/ado/2009/02/edm/annotation" '
         'xmlns:customannotation="http://schemas.microsoft.com/ado/2013/11/edm/customannotation" '
         'xmlns="http://schemas.microsoft.com/ado/2009/11/edm">' % NS]
    for e, t, cols in E:
        o.append('        <EntityType Name="%s">' % e)
        o.append('          <Key>\n            <PropertyRef Name="%s" />\n          </Key>' % key(e))
        o += [csdl_prop(c) for c in cols]
        for name, rel, frm, to, coll in navs(e):
            o.append('          <NavigationProperty Name="%s" Relationship="Self.%s" FromRole="%s" ToRole="%s" />' % (name, rel, frm, to))
        o.append('        </EntityType>')
    for a in A:
        n = assoc_name(a)
        pm = '0..1' if col(a[2], a[3])[3] else '1'
        o.append('        <Association Name="%s">' % n)
        o.append('          <End Role="%s" Type="Self.%s" Multiplicity="%s" />' % (a[0], a[0], pm))
        o.append('          <End Role="%s" Type="Self.%s" Multiplicity="*" />' % (a[2], a[2]))
        o.append('          <ReferentialConstraint>\n            <Principal Role="%s">\n              <PropertyRef Name="%s" />\n'
                 '            </Principal>\n            <Dependent Role="%s">\n              <PropertyRef Name="%s" />\n'
                 '            </Dependent>\n          </ReferentialConstraint>' % (a[0], a[1], a[2], a[3]))
        o.append('        </Association>')
    o.append('        <EntityContainer Name="%s" annotation:LazyLoadingEnabled="true">' % CTX)
    for e, t, cols in E:
        o.append('          <EntitySet Name="%s" EntityType="Self.%s" />' % (t, e))
    for a in A:
        n = assoc_name(a)
        o.append('          <AssociationSet Name="%s" Association="Self.%s">\n            <End Role="%s" EntitySet="%s" />\n'
                 '            <End Role="%s" EntitySet="%s" />\n          </AssociationSet>' % (n, n, a[0], table[a[0]], a[2], table[a[2]]))
    o.append('        </EntityContainer>\n      </Schema>')
    return '\n'.join(o)


def msl():
    o = ['      <Mapping Space="C-S" xmlns="http://schemas.microsoft.com/ado/2009/11/mapping/cs">',
         '        <EntityContainerMapping StorageEntityContainer="%sStoreContainer" CdmEntityContainer="%s">' % (NS, CTX)]
    for e, t, cols in E:
        o.append('          <EntitySetMapping Name="%s">\n            <EntityTypeMapping TypeName="%s.%s">\n'
                 '              <MappingFragment StoreEntitySet="%s">' % (t, NS, e, t))
        o += ['                <ScalarProperty Name="%s" ColumnName="%s" />' % (c[0], c[0]) for c in cols]
        o.append('              </MappingFragment>\n            </EntityTypeMapping>\n          </EntitySetMapping>')
    o.append('        </EntityContainerMapping>\n      </Mapping>')
    return '\n'.join(o)


edmx = '''<?xml version="1.0" encoding="utf-8"?>
<edmx:Edmx Version="3.0" xmlns:edmx="http://schemas.microsoft.com/ado/2009/11/edmx">
  <!-- EF Runtime content -->
  <edmx:Runtime>
    <!-- SSDL content -->
    <edmx:StorageModels>
%s
    </edmx:StorageModels>
    <!-- CSDL content -->
    <edmx:ConceptualModels>
%s
    </edmx:ConceptualModels>
    <!-- C-S mapping content -->
    <edmx:Mappings>
%s
    </edmx:Mappings>
  </edmx:Runtime>
  <!-- EF Designer content (DO NOT EDIT MANUALLY BELOW HERE) -->
  <Designer xmlns="http://schemas.microsoft.com/ado/2009/11/edmx">
    <Connection>
      <DesignerInfoPropertySet>
        <DesignerProperty Name="MetadataArtifactProcessing" Value="EmbedInOutputAssembly" />
      </DesignerInfoPropertySet>
    </Connection>
    <Options>
      <DesignerInfoPropertySet>
        <DesignerProperty Name="ValidateOnBuild" Value="true" />
        <DesignerProperty Name="EnablePluralization" Value="false" />
        <DesignerProperty Name="IncludeForeignKeysInModel" Value="true" />
        <DesignerProperty Name="UseLegacyProvider" Value="false" />
        <DesignerProperty Name="CodeGenerationStrategy" Value="None" />
      </DesignerInfoPropertySet>
    </Options>
    <!-- Diagram content (shape and connector positions) -->
    <Diagrams></Diagrams>
  </Designer>
</edmx:Edmx>
''' % (ssdl(), csdl(), msl())
open(os.path.join(OUT, NS + '.edmx'), 'w', encoding='utf-8').write(edmx)

shapes = '\n'.join('        <EntityTypeShape EntityType="%s.%s" Width="1.5" PointX="%.2f" PointY="%.2f" IsExpanded="true" />'
                   % (NS, e[0], 0.75 + (i % 4) * 3, 0.75 + (i // 4) * 4.5) for i, e in enumerate(E))
assocs = '\n'.join('        <AssociationConnector Association="%s.%s" />' % (NS, assoc_name(a)) for a in A)
open(os.path.join(OUT, NS + '.edmx.diagram'), 'w', encoding='utf-8').write('''<?xml version="1.0" encoding="utf-8"?>
<edmx:Edmx Version="3.0" xmlns:edmx="http://schemas.microsoft.com/ado/2009/11/edmx">
 <!-- EF Designer content (DO NOT EDIT MANUALLY BELOW HERE) -->
  <edmx:Designer xmlns="http://schemas.microsoft.com/ado/2009/11/edmx">
    <!-- Diagram content (shape and connector positions) -->
    <edmx:Diagrams>
      <Diagram DiagramId="%s" Name="Diagram1">
%s
%s
      </Diagram>
    </edmx:Diagrams>
  </edmx:Designer>
</edmx:Edmx>
''' % (uuid.uuid4().hex, shapes, assocs))

# ---------------------------------------------------------------- C# classes (same shape as the EF6 T4 output)
HEADER = '''//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

'''
CS = {'int': 'int', 'nvarchar': 'string', 'nchar': 'string', 'money': 'decimal', 'decimal': 'decimal', 'bit': 'bool',
      'date': 'System.DateTime', 'datetime': 'System.DateTime', 'float': 'double', 'rowversion': 'byte[]'}
for e, t, cols in E:
    colls = [n for n in navs(e) if n[4]]
    refs = [n for n in navs(e) if not n[4]]
    o = [HEADER + 'namespace LawyerProject.DB\n{\n    using System;\n    using System.Collections.Generic;\n    ',
         '    public partial class %s\n    {' % e]
    if colls:
        o.append('        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]')
        o.append('        public %s()\n        {' % e)
        o += ['            this.%s = new HashSet<%s>();' % (n[0], n[3]) for n in colls]
        o.append('        }\n    ')
    for n, st, l, null, f in cols:
        ty = CS[st]
        if null and ty not in ('string', 'byte[]'):
            ty = 'Nullable<%s>' % ty
        o.append('        public %s %s { get; set; }' % (ty, n))
    if colls or refs:
        o.append('    ')
    for n in refs:
        o.append('        public virtual %s %s { get; set; }' % (n[3], n[0]))
    for n in colls:
        o.append('        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]')
        o.append('        public virtual ICollection<%s> %s { get; set; }' % (n[3], n[0]))
    o.append('    }\n}\n')
    open(os.path.join(OUT, e + '.cs'), 'w', encoding='utf-8').write('\n'.join(o))

ctx = [HEADER + 'namespace LawyerProject.DB\n{\n    using System;\n    using System.Data.Entity;\n    using System.Data.Entity.Infrastructure;\n    ',
       '    public partial class %s : DbContext\n    {\n        public %s()\n            : base("name=%s")\n        {\n        }\n    ' % (CTX, CTX, CTX),
       '        protected override void OnModelCreating(DbModelBuilder modelBuilder)\n        {\n            throw new UnintentionalCodeFirstException();\n        }\n    ']
ctx += ['        public virtual DbSet<%s> %s { get; set; }' % (e, t) for e, t, c in E]
ctx.append('    }\n}\n')
open(os.path.join(OUT, NS + '.Context.cs'), 'w', encoding='utf-8').write('\n'.join(ctx))
print('model: %d entities, %d associations' % (len(E), len(A)))
