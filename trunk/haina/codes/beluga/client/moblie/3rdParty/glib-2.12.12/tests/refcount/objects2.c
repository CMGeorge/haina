#include <unistd.h>
#include <glib.h>
#include <glib-object.h>

#define G_TYPE_TEST               (g_test_get_type ())
#define G_TEST(test)              (G_TYPE_CHECK_INSTANCE_CAST ((test), G_TYPE_TEST, GTest))
#define G_IS_TEST(test)           (G_TYPE_CHECK_INSTANCE_TYPE ((test), G_TYPE_TEST))
#define G_TEST_CLASS(tclass)      (G_TYPE_CHECK_CLASS_CAST ((tclass), G_TYPE_TEST, GTestClass))
#define G_IS_TEST_CLASS(tclass)   (G_TYPE_CHECK_CLASS_TYPE ((tclass), G_TYPE_TEST))
#define G_TEST_GET_CLASS(test)    (G_TYPE_INSTANCE_GET_CLASS ((test), G_TYPE_TEST, GTestClass))

typedef struct _GTest GTest;
typedef struct _GTestClass GTestClass;

struct _GTest
{
  GObject object;
};

struct _GTestClass
{
  GObjectClass parent_class;
};

static GType g_test_get_type (void);

static void g_test_class_init (GTestClass * klass);
static void g_test_init (GTest * test);
static void g_test_dispose (GObject * object);

static GObjectClass *parent_class = NULL;

static GType
g_test_get_type (void)
{
  static GType test_type = 0;

  if (!test_type) {
    static const GTypeInfo test_info = {
      sizeof (GTestClass),
      NULL,
      NULL,
      (GClassInitFunc) g_test_class_init,
      NULL,
      NULL,
      sizeof (GTest),
      0,
      (GInstanceInitFunc) g_test_init,
      NULL
    };

    test_type = g_type_register_static (G_TYPE_OBJECT, "GTest",
        &test_info, 0);
  }
  return test_type;
}

static void
g_test_class_init (GTestClass * klass)
{
  GObjectClass *gobject_class;

  gobject_class = (GObjectClass *) klass;

  parent_class = g_type_class_ref (G_TYPE_OBJECT);

  gobject_class->dispose = g_test_dispose;
}

static void
g_test_init (GTest * test)
{
  g_print ("init %p\n", test);
}

static void
g_test_dispose (GObject * object)
{
  GTest *test;

  test = G_TEST (object);

  g_print ("dispose %p!\n", object);

  G_OBJECT_CLASS (parent_class)->dispose (object);
}

static void
g_test_do_refcount (GTest * test)
{
  static guint i = 1;
  if (i++ % 100000 == 0)
    g_print (".");
  g_object_ref (test); 
  g_object_unref (test); 
}

int
main (int argc, char **argv)
{
  gint i;
  GTest *test;

  g_thread_init (NULL);
  g_print ("START: %s\n", argv[0]);
  g_log_set_always_fatal (G_LOG_LEVEL_WARNING | G_LOG_LEVEL_CRITICAL | g_log_set_always_fatal (G_LOG_FATAL_MASK));
  g_type_init ();

  test = g_object_new (G_TYPE_TEST, NULL);

  for (i=0; i<100000000; i++) {
    g_test_do_refcount (test);
  }

  g_print ("\n");
  
  return 0;
}
