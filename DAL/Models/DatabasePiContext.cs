using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace DAL.Models;

public partial class DatabasePiContext : DbContext
{
    public DatabasePiContext()
    {
    }

    public DatabasePiContext(DbContextOptions<DatabasePiContext> options)
        : base(options)
    {
    }

    public virtual DbSet<CatEstadoCitum> CatEstadoCita { get; set; }

    public virtual DbSet<CatInfoMedico> CatInfoMedicos { get; set; }

    public virtual DbSet<CatPlantaCitum> CatPlantaCita { get; set; }

    public virtual DbSet<CatRolUsuario> CatRolUsuarios { get; set; }

    public virtual DbSet<CatSalaCitum> CatSalaCita { get; set; }

    public virtual DbSet<Cita> Citas { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Server=localhost;Port=5432;Database=databasePI;User Id=postgres;Password=Juancarbc2001");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CatEstadoCitum>(entity =>
        {
            entity.HasKey(e => e.EstadoCita).HasName("cat_estado_cita_pkey");

            entity.ToTable("cat_estado_cita", "dlk_controlacceso");

            entity.Property(e => e.EstadoCita)
                .HasColumnType("character varying")
                .HasColumnName("estado_cita");
            entity.Property(e => e.DescEstadoCita)
                .HasColumnType("character varying")
                .HasColumnName("desc_estado_cita");
            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("id");
        });

        modelBuilder.Entity<CatInfoMedico>(entity =>
        {
            entity.HasKey(e => e.NombreMedico).HasName("cat_info_medico_pkey");

            entity.ToTable("cat_info_medico", "dlk_controlacceso");

            entity.Property(e => e.NombreMedico)
                .HasColumnType("character varying")
                .HasColumnName("nombre_medico");
            entity.Property(e => e.CodPlanta)
                .HasColumnType("character varying")
                .HasColumnName("cod_planta");
            entity.Property(e => e.CodSala)
                .HasColumnType("character varying")
                .HasColumnName("cod_sala");
            entity.Property(e => e.Especialidad)
                .HasColumnType("character varying")
                .HasColumnName("especialidad");
            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("id");
        });

        modelBuilder.Entity<CatPlantaCitum>(entity =>
        {
            entity.HasKey(e => e.CodPlanta).HasName("cat_planta_cita_pkey");

            entity.ToTable("cat_planta_cita", "dlk_controlacceso");

            entity.Property(e => e.CodPlanta)
                .HasColumnType("character varying")
                .HasColumnName("cod_planta");
            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("id");
            entity.Property(e => e.NombrePlanta)
                .HasColumnType("character varying")
                .HasColumnName("nombre_planta");
        });

        modelBuilder.Entity<CatRolUsuario>(entity =>
        {
            entity.HasKey(e => e.NivelAcceso).HasName("cat_rol_usuario_pkey");

            entity.ToTable("cat_rol_usuario", "dlk_controlacceso");

            entity.Property(e => e.NivelAcceso)
                .ValueGeneratedNever()
                .HasColumnName("nivel_acceso");
            entity.Property(e => e.ControlAcceso)
                .HasColumnType("character varying")
                .HasColumnName("control_acceso");
            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("id");
        });

        modelBuilder.Entity<CatSalaCitum>(entity =>
        {
            entity.HasKey(e => e.CodSala).HasName("cat_sala_cita_pkey");

            entity.ToTable("cat_sala_cita", "dlk_controlacceso");

            entity.Property(e => e.CodSala)
                .HasColumnType("character varying")
                .HasColumnName("cod_sala");
            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("id");
            entity.Property(e => e.NombreSala)
                .HasColumnType("character varying")
                .HasColumnName("nombre_sala");
        });

        modelBuilder.Entity<Cita>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("citas_pkey");

            entity.ToTable("citas", "dlk_controlacceso");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Asunto)
                .HasMaxLength(255)
                .HasColumnName("asunto");
            entity.Property(e => e.CodPlanta)
                .HasMaxLength(255)
                .HasColumnName("cod_planta");
            entity.Property(e => e.CodSala)
                .HasMaxLength(255)
                .HasColumnName("cod_sala");
            entity.Property(e => e.Enfermedad)
                .HasMaxLength(255)
                .HasColumnName("enfermedad");
            entity.Property(e => e.EstadoCita)
                .HasMaxLength(255)
                .HasColumnName("estado_cita");
            entity.Property(e => e.Fecha).HasColumnName("fecha");
            entity.Property(e => e.Hora).HasColumnName("hora");
            entity.Property(e => e.MdDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("md_date");
            entity.Property(e => e.MdUuid)
                .HasMaxLength(255)
                .HasColumnName("md_uuid");
            entity.Property(e => e.NombreMedico)
                .HasMaxLength(255)
                .HasColumnName("nombre_medico");
            entity.Property(e => e.NombrePaciente)
                .HasMaxLength(255)
                .HasColumnName("nombre_paciente");
            entity.Property(e => e.Sintomas)
                .HasMaxLength(1000)
                .HasColumnName("sintomas");
            entity.Property(e => e.Solucion)
                .HasMaxLength(255)
                .HasColumnName("solucion");

            entity.HasOne(d => d.CodPlantaNavigation).WithMany(p => p.Cita)
                .HasForeignKey(d => d.CodPlanta)
                .HasConstraintName("planta_fk");

            entity.HasOne(d => d.CodSalaNavigation).WithMany(p => p.Cita)
                .HasForeignKey(d => d.CodSala)
                .HasConstraintName("sala_fk");

            entity.HasOne(d => d.NombreMedicoNavigation).WithMany(p => p.CitaNombreMedicoNavigations)
                .HasForeignKey(d => d.NombreMedico)
                .HasConstraintName("medico_usuarios_fk");

            entity.HasOne(d => d.NombreMedico1).WithMany(p => p.Cita)
                .HasForeignKey(d => d.NombreMedico)
                .HasConstraintName("medico_info_fk");

            entity.HasOne(d => d.NombrePacienteNavigation).WithMany(p => p.CitaNombrePacienteNavigations)
                .HasForeignKey(d => d.NombrePaciente)
                .HasConstraintName("paciente_usuarios_fk");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.NombreCompleto).HasName("usuarios_pkey");

            entity.ToTable("usuarios", "dlk_controlacceso");

            entity.Property(e => e.NombreCompleto)
                .HasColumnType("character varying")
                .HasColumnName("nombre_completo");
            entity.Property(e => e.Contrasenya)
                .HasColumnType("character varying")
                .HasColumnName("contrasenya");
            entity.Property(e => e.Email)
                .HasColumnType("character varying")
                .HasColumnName("email");
            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("id");
            entity.Property(e => e.MdDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("md_date");
            entity.Property(e => e.MdUuid)
                .HasColumnType("character varying")
                .HasColumnName("md_uuid");
            entity.Property(e => e.Movil)
                .HasColumnType("character varying")
                .HasColumnName("movil");
            entity.Property(e => e.Rol).HasColumnName("rol");

            entity.HasOne(d => d.RolNavigation).WithMany(p => p.Usuarios)
                .HasForeignKey(d => d.Rol)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("rol_fk");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
